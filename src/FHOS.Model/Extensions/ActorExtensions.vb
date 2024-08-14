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
                actor.Dialog = New OutOfFuelDialog(actor, actor.Dialog)
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
        Dim destinations = otherActor.Location.GetEmptyCardinalNeighbors()
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
                actor.Dialog = tank.Descriptor.Dialogs(actor, tank, Nothing)(DialogChoices.RefillOxygen)
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
        If actor.Descriptor.HasEquipSlot(equipSlot) AndAlso item.Descriptor.CanEquip(equipSlot) AndAlso actor.Equipment.GetSlot(equipSlot) Is Nothing Then
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
        deliveryItem.SetOriginPlanet(planet)
        Dim planetVicinity = planet.SingleParent(GroupTypes.PlanetVicinity)
        Dim faction = planetVicinity.SingleParent(GroupTypes.Faction)
        Dim candidates = faction.
            ChildrenOfType(GroupTypes.PlanetVicinity).
            Where(Function(x) x.Id <> planetVicinity.Id).
            Select(Function(x) x.Children.Single(Function(y) y.EntityType = GroupTypes.Planet))
        Dim destination = RNG.FromEnumerable(candidates)
        deliveryItem.Metadatas(MetadataTypes.EntityName) = $"{GenerateAdverb()} {GenerateAdjective()} {GenerateNoun()}"
        deliveryItem.Metadatas(MetadataTypes.Recipient) = $"{GenerateRecipientName()} the {GenerateRecipientJob()}"
        deliveryItem.SetDestinationPlanet(destination)
        deliveryItem.SetJoolsReward(RNG.RollDice("5d20"))
        deliveryItem.SetReputationBonus(1)
        deliveryItem.SetReputationPenalty(-5)
    End Sub

    Private jobs As IReadOnlyList(Of String) =
        New List(Of String) From
        {
        "Starship Engineer",
        "Quantum Physicist",
        "Terraforming Specialist",
        "Cybernetics Surgeon",
        "Galactic Diplomat",
        "Stellar Cartographer",
        "Bioinformatics Analyst",
        "Interstellar Trader",
        "Artificial Intelligence Ethicist",
        "Astrobiologist",
        "Nanotechnology Architect",
        "Exo-Law Enforcement Officer",
        "Gravity Manipulator",
        "Planetary Governor",
        "Holographic Artist",
        "Space Miner",
        "Drone Operator",
        "Energy Harvesting Technician",
        "Virtual Reality Designer",
        "Time Dilation Theorist",
        "Cloning Technician",
        "Subspace Communications Officer",
        "Genetic Enhancement Specialist"
    }
    Private firstNames As IReadOnlyList(Of String) =
        New List(Of String) From
        {
            "Gorachan",
            "Samuli",
            "David",
            "Orin",
            "Lyra",
            "Jaxon",
            "Zara",
            "Talon",
            "Mira",
            "Kael",
            "Nova",
            "Vera",
            "Dax",
            "Seren",
            "Ryn",
            "Eris",
            "Kara",
            "Thorne",
            "Xen",
            "Isla",
            "Cade",
            "Nia",
            "Rook"
    }
    Private lastNames As IReadOnlyList(Of String) =
        New List(Of String) From
        {
            "Valken",
            "Nex",
            "Kyre",
            "Korrin",
            "Rho",
            "Aethon",
            "Draven",
            "Elara",
            "Synn",
            "Voss",
            "Vael",
            "Kaelor",
            "Nyx",
            "Zhen",
            "Varek",
            "Raith",
            "Thorne",
            "Arvon",
            "Solis",
            "Vire",
            "Vantros",
            "Kevar",
            "Draylen"
    }
    Private Function GenerateRecipientJob() As Object
        Return RNG.FromEnumerable(jobs)
    End Function

    Private Function GenerateRecipientName() As Object
        Return $"{RNG.FromEnumerable(firstNames)} {RNG.FromEnumerable(lastNames)}"
    End Function

    Private nouns As IReadOnlyList(Of String) =
        New List(Of String) From
        {
            "Quantum Batteries",
            "Bio-enhancement Serums",
            "Nanobot Swarms",
            "Cryogenic Stasis Pods",
            "Plasma Rifles",
            "Terraforming Modules",
            "Holographic Projectors",
            "Fusion Cores",
            "Genetic Material Samples",
            "Cybernetic Implants",
            "Antimatter Containment Units",
            "Starship Components",
            "Exoskeleton Suits",
            "Medical Nanogel",
            "Encrypted Data Cores",
            "Alien Artifacts",
            "Subspace Communication Relays",
            "Atmospheric Stabilizers",
            "Portable Shield Generators",
            "Interstellar Navigation Charts",
            "Mind Interface Devices",
            "Graviton Manipulators",
            "Zero-Point Energy Cells"
        }

    Private adjectives As IReadOnlyList(Of String) =
        New List(Of String) From
        {
            "Resilient",
            "Mysterious",
            "Luminescent",
            "Vast",
            "Agile",
            "Intricate",
            "Ancient",
            "Formidable",
            "Elegant",
            "Futuristic",
            "Stealthy",
            "Versatile",
            "Rugged",
            "Peculiar",
            "Vibrant",
            "Nomadic",
            "Sophisticated",
            "Expansive",
            "Grim",
            "Innovative",
            "Intrepid",
            "Surreal",
            "Stoic"
        }

    Private adverbs As IReadOnlyList(Of String) =
        New List(Of String) From
        {
            "Swiftly",
            "Cautiously",
            "Effortlessly",
            "Vigorously",
            "Silently",
            "Reluctantly",
            "Rapidly",
            "Gracefully",
            "Mysteriously",
            "Precisely",
            "Fiercely",
            "Steadily",
            "Eagerly",
            "Curiously",
            "Quietly",
            "Boldly",
            "Patiently",
            "Carefully",
            "Relentlessly",
            "Eerily",
            "Unexpectedly",
            "Diligently",
            "Calmly"
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
    <Extension>
    Friend Function UpdateReputations(actor As IActor, reputation As Integer, planet As IGroup, Optional groups As IEnumerable(Of IGroup) = Nothing) As IEnumerable(Of IGroup)
        Dim planetVicinity = planet.SingleParent(GroupTypes.PlanetVicinity)
        Dim starSystem = planetVicinity.SingleParent(GroupTypes.StarSystem)
        Dim faction = planetVicinity.SingleParent(GroupTypes.Faction)
        If groups Is Nothing OrElse Not groups.Any(Function(x) x.Id = planetVicinity.Id) Then
            actor.AddReputation(planetVicinity, reputation)
        End If
        If groups Is Nothing OrElse Not groups.Any(Function(x) x.Id = starSystem.Id) Then
            actor.AddReputation(starSystem, reputation)
        End If
        If groups Is Nothing OrElse Not groups.Any(Function(x) x.Id = faction.Id) Then
            actor.AddReputation(faction, reputation)
        End If
        Dim result As New List(Of IGroup) From {planetVicinity, starSystem, faction}
        If groups IsNot Nothing Then
            result.AddRange(result)
        End If
        Return result
    End Function
    <Extension>
    Friend Function GetWorstReputation(actor As IActor, planet As IGroup) As Integer
        Dim planetVicinity = planet.SingleParent(GroupTypes.PlanetVicinity)
        Dim starSystem = planetVicinity.SingleParent(GroupTypes.StarSystem)
        Dim faction = planetVicinity.SingleParent(GroupTypes.Faction)
        Return {
            If(actor.GetReputation(planetVicinity), 0),
            If(actor.GetReputation(starSystem), 0),
            If(actor.GetReputation(faction), 0)
        }.Min
    End Function
    <Extension>
    Friend Function CanChangeEquipment(actor As IActor, target As IActor) As Boolean
        Return target.ChangeableEquipmentSlots.Any
    End Function
    <Extension>
    Friend Function ChangeableEquipmentSlots(actor As IActor) As IEnumerable(Of String)
        Return actor.
                Equipment.
                GetRequiredSlots.
                Where(Function(x) actor.Inventory.Items.Any(Function(y) y.Descriptor.CanEquip(x)))
    End Function
    <Extension>
    Friend Function CanInstallEquipment(actor As IActor, target As IActor) As Boolean
        Return target.InstallableEquipmentSlots.Any
    End Function
    <Extension>
    Friend Function InstallableEquipmentSlots(actor As IActor) As IEnumerable(Of String)
        Return actor.
                Equipment.
                GetInstallableSlots.
                Where(Function(x) actor.Inventory.Items.Any(Function(y) y.Descriptor.CanEquip(x)))
    End Function
    <Extension>
    Friend Function CanUninstallEquipment(actor As IActor, target As IActor) As Boolean
        Return target.UninstallableEquipmentSlots.Any
    End Function
    <Extension>
    Friend Function UninstallableEquipmentSlots(actor As IActor) As IEnumerable(Of String)
        Return actor.
                Equipment.
                GetUninstallableSlots
    End Function
End Module
