Imports System.Runtime.CompilerServices
Imports FHOS.Persistence
Imports SPLORR.Game

Friend Module ActorExtensions
    <Extension>
    Function LifeSupport(actor As IActor) As IStore
        Return actor.Yokes.Store(YokeTypes.LifeSupport)
    End Function
    <Extension>
    Function FuelTank(actor As IActor) As IStore
        Return actor.Yokes.Store(YokeTypes.FuelTank)
    End Function
    <Extension>
    Function Faction(actor As IActor) As IGroup
        Return actor.Yokes.Group(YokeTypes.Faction)
    End Function
    <Extension>
    Function HomePlanet(actor As IActor) As IGroup
        Return actor.Yokes.Group(YokeTypes.HomePlanet)
    End Function
    <Extension>
    Function ItemEquipped(actor As IActor, equipSlot As String) As IItem
        Return actor.Equipment.GetSlot(equipSlot)
    End Function
    <Extension>
    Sub ClearInteractor(actor As IActor)
        actor.Yokes.Actor(YokeTypes.Interactor) = Nothing
    End Sub
    <Extension>
    Function Interactor(actor As IActor) As IActor
        Return actor.Yokes.Actor(YokeTypes.Interactor)
    End Function
    <Extension>
    Function CostumeDescriptor(actor As IActor) As CostumeTypeDescriptor
        Return CostumeTypes.Descriptors(actor.Costume)
    End Function
    <Extension>
    Function Descriptor(actor As IActor) As ActorTypeDescriptor
        Return ActorTypes.Descriptors(actor.EntityType)
    End Function

    <Extension>
    Function NeedsOxygen(actor As IActor) As Boolean
        Return actor.LifeSupport.NeedsTopOff
    End Function

    <Extension>
    Sub Move(actor As IActor, facing As Integer)
        actor.Statistics(StatisticTypes.Facing) = facing
        Dim delta = FacingTypes.Descriptors(facing).Delta
        If Not CanMove(actor) Then
            Return
        End If
        DoTurn(actor)
        DoFuelConsumption(actor)
        Dim location = actor.Location
        Dim nextColumn = location.Position.Column + delta.X
        Dim nextRow = location.Position.Row + delta.Y
        Dim map = location.Map
        Dim nextLocation = map.GetLocation(nextColumn, nextRow)
        If nextLocation Is Nothing Then
            Return
        End If
        If nextLocation.Actor IsNot Nothing Then
            actor.Yokes.Actor(YokeTypes.Interactor) = nextLocation.Actor
            Return
        End If
        If Not CanEnterLocation(nextLocation, actor) Then
            Return
        End If
        SetLocation(actor, nextLocation)
    End Sub

    <Extension>
    Function CanEnterLocation(nextLocation As ILocation, actor As IActor) As Boolean
        Dim descriptor = LocationTypes.Descriptors(nextLocation.EntityType)
        Return descriptor.IsEnterable AndAlso actor IsNot Nothing
    End Function

    <Extension>
    Sub DoFuelConsumption(actor As IActor)
        If actor.FuelTank IsNot Nothing Then
            actor.FuelTank.CurrentValue -= 1
            If actor.FuelTank.CurrentValue = actor.FuelTank.MinimumValue.Value Then
                actor.Universe.Messages.Add("Out of Fuel!",
                    {
                        ("You are out of fuel!", Hues.LightGray),
                        ("", Hues.LightGray),
                        ("To send a distress signal,", Hues.LightGray),
                        ("press [Space/Enter] from the NAV SCREEN", Hues.LightGray),
                        ("then choose 'Distress Signal'", Hues.LightGray)
                    })
            End If
        End If
    End Sub

    <Extension>
    Function CanMove(actor As IActor) As Boolean
        Return actor.FuelTank Is Nothing OrElse
                AvatarModel.FromActor(actor).Vessel.FuelQuantity.Value > 0
    End Function

    <Extension>
    Sub SetLocation(actor As IActor, location As ILocation)
        actor.Location = location
    End Sub

    <Extension>
    Sub GoToOtherActor(actor As IActor, otherActor As IActor, postAction As Action(Of Boolean, IActor))
        Dim destinations = otherActor.Location.GetEmptyNeighbors()
        If Not destinations.Any Then
            postAction(False, otherActor)
            Return
        End If
        Dim destination = RNG.FromEnumerable(destinations)
        SetLocation(actor, destination)
        postAction(True, otherActor)
    End Sub

    <Extension>
    Sub DoTurn(actor As IActor)
        actor.Universe.Turn += 1
        Dim store = actor.LifeSupport
        store.CurrentValue -= 1
        If store.CurrentValue = store.MinimumValue Then
            Dim tank = actor.Inventory.Items.FirstOrDefault(Function(x) x.EntityType = ItemTypes.OxygenTank)
            If tank IsNot Nothing Then
                ItemTypes.Descriptors(ItemTypes.OxygenTank).Use(actor, tank)
            End If
        End If
    End Sub
    <Extension>
    Sub SetStarSystem(actor As IActor, starSystemGroup As IGroup)
        Dim oldStarSystem = actor.Yokes.Group(YokeTypes.StarSystem)
        If starSystemGroup IsNot Nothing AndAlso (oldStarSystem Is Nothing OrElse oldStarSystem.Id <> starSystemGroup.Id) Then
            starSystemGroup.Statistics(StatisticTypes.VisitCount) += 1
        End If
        actor.Yokes.Group(YokeTypes.StarSystem) = starSystemGroup
    End Sub
    <Extension>
    Function Equip(actor As IActor, equipSlot As String, item As IItem) As Boolean
        If item Is Nothing Then
            Throw New NotImplementedException("TODO: when item is nothing... we are UNEQUIPPING")
        End If
        If actor.Descriptor.HasEquipSlot(equipSlot) AndAlso item.Descriptor.EquipSlot = equipSlot AndAlso actor.Equipment.GetSlot(equipSlot) Is Nothing Then
            actor.Equipment.Equip(equipSlot, item)
            item.OnEquip(actor)
            Return True
        End If
        Return False
    End Function
    <Extension>
    Sub GenerateDeliveryMission(actor As Persistence.IActor)
        Dim deliveryItem = actor.Universe.Factory.CreateItem(ItemTypes.Delivery)
        actor.Inventory.Add(deliveryItem)
        Dim planet = actor.Yokes.Group(YokeTypes.Planet)
        deliveryItem.Statistics(StatisticTypes.OriginPlanetId) = planet.Id
        Dim planetVicinity = planet.Parents.Single(Function(x) x.EntityType = GroupTypes.PlanetVicinity)
        Dim faction = planetVicinity.Parents.Single(Function(x) x.EntityType = GroupTypes.Faction)
        Dim candidates = faction.
            ChildrenOfType(GroupTypes.PlanetVicinity).
            Where(Function(x) x.Id <> planetVicinity.Id).
            Select(Function(x) x.Children.Single(Function(y) y.EntityType = GroupTypes.Planet))
        Dim destination = RNG.FromEnumerable(candidates)
        deliveryItem.Metadatas(MetadataTypes.EntityName) = $"{GenerateAdverb()} {GenerateAdjective()} {GenerateNoun()}"
        deliveryItem.Metadatas(MetadataTypes.Recipient) = $"{GenerateRecipientName()} the {GenerateRecipientJob()}"
        deliveryItem.SetDestinationPlanet(destination)
        deliveryItem.SetJoolsReward(10)
        deliveryItem.SetReputationBonus(1)
        deliveryItem.SetReputationPenalty(-5)
    End Sub

    Private jobs As IReadOnlyList(Of String) =
        New List(Of String) From
        {
            "Pirate",
            "Merkinsmith",
            "Harlot"
        }
    Private names As IReadOnlyList(Of String) =
        New List(Of String) From
        {
            "Gorachan",
            "Samuli",
            "David"
        }
    Private Function GenerateRecipientJob() As Object
        Return RNG.FromEnumerable(jobs)
    End Function

    Private Function GenerateRecipientName() As Object
        Return RNG.FromEnumerable(names)
    End Function

    Private nouns As IReadOnlyList(Of String) =
        New List(Of String) From
        {
            "thingie",
            "merkin",
            "marital aid"
        }

    Private adjectives As IReadOnlyList(Of String) =
        New List(Of String) From
        {
            "salty",
            "unwashed",
            "used"
        }

    Private adverbs As IReadOnlyList(Of String) =
        New List(Of String) From
        {
            "extremely",
            "oddly",
            "woefully"
        }

    Private Function GenerateNoun() As String
        Return RNG.FromList(nouns.ToList)
    End Function

    Private Function GenerateAdjective() As Object
        Return RNG.FromList(adjectives.ToList)
    End Function

    Private Function GenerateAdverb() As Object
        Return RNG.FromList(adverbs.ToList)
    End Function
End Module
