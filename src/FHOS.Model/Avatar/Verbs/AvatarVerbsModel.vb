Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class AvatarVerbsModel
    Inherits BaseAvatarModel
    Implements IAvatarVerbsModel

    Protected Sub New(actor As IActor)
        MyBase.New(actor)
    End Sub

    Private ReadOnly verbTable As IReadOnlyDictionary(Of String, (isAvailable As Func(Of Boolean), perform As Action)) =
        New Dictionary(Of String, (isAvailable As Func(Of Boolean), perform As Action)) From
        {
            {VerbTypes.Status, (Function() True, Sub() Return)},
            {VerbTypes.KnownPlaces, (Function() actor.KnownPlaces.HasAny, Sub() Return)},
            {VerbTypes.RefillOxygen, (Function() CanRefillOxygen, AddressOf RefillOxygen)},
            {VerbTypes.Refuel, (Function() CanRefillFuel, AddressOf Refuel)},
            {VerbTypes.EnterWormhole, (Function() CanEnterWormhole, AddressOf EnterWormhole)},
            {VerbTypes.EnterOrbit, (Function() CanEnterOrbit, AddressOf EnterOrbit)},
            {VerbTypes.EnterStarSystem, (Function() CanEnterStarSystem, AddressOf EnterStarSystem)},
            {VerbTypes.ApproachPlanetVicinity, (Function() CanApproachPlanetVicinity, AddressOf ApproachPlanetVicinity)},
            {VerbTypes.ApproachStarVicinity, (Function() CanApproachStarVicinity, AddressOf ApproachStarVicinity)},
            {VerbTypes.DistressSignal, (Function() Not CanMove, AddressOf DistressSignal)},
            {VerbTypes.MoveUp, (Function() CanMove, Sub() Move(Facing.Up))},
            {VerbTypes.MoveRight, (Function() CanMove, Sub() Move(Facing.Right))},
            {VerbTypes.MoveDown, (Function() CanMove, Sub() Move(Facing.Down))},
            {VerbTypes.MoveLeft, (Function() CanMove, Sub() Move(Facing.Left))},
            {VerbTypes.SPLORRPedia, (Function() True, Sub() Return)},
            {VerbTypes.Crew, (Function() actor.Family.HasChildren, Sub() Return)},
            {VerbTypes.Vessel, (Function() actor.Family.Parent IsNot Nothing, Sub() Return)}
        }

    Private Sub Move(facing As Integer)
        actor.Facing = facing
        Dim delta = Persistence.Facing.Deltas(facing)
        If Not CanMove Then
            Return
        End If
        DoTurn()
        DoFuelConsumption()
        Dim location = actor.Location
        Dim nextColumn = location.Column + delta.X
        Dim nextRow = location.Row + delta.Y
        Dim map = location.Map
        Dim nextLocation = map.GetLocation(nextColumn, nextRow)
        If nextLocation Is Nothing Then
            Return
        End If
        If nextLocation.Actor IsNot Nothing Then
            actor.Interactor = nextLocation.Actor
            Return
        End If
        If nextLocation.HasTargetLocation Then
            nextLocation = nextLocation.TargetLocation
        End If
        SetLocation(nextLocation)
    End Sub

    Private Sub DoFuelConsumption()
        If actor.FuelTank IsNot Nothing Then
            actor.FuelTank.CurrentValue -= 1
            If actor.FuelTank.CurrentValue = actor.FuelTank.MinimumValue.Value Then
                actor.Tutorial.Add(TutorialTypes.OutOfFuel)
            End If
        End If
    End Sub

    Private Sub DistressSignal()
        Dim fuelAdded = actor.FuelTank.MaximumValue.Value - actor.FuelTank.CurrentValue
        Dim fuelPrice = 1 'TODO: don't just pick a magic number!
        Dim price = fuelPrice * fuelAdded
        actor.FuelTank.CurrentValue = actor.FuelTank.MaximumValue.Value
        actor.Wallet.CurrentValue -= fuelAdded * fuelPrice
        actor.Universe.Messages.Add(
            "Emergency Refuel",
            ($"Added {fuelAdded} fuel!", Hues.Black),
            ($"Price {price} jools!", Hues.Black))
    End Sub

    Private ReadOnly Property CanMove As Boolean
        Get
            Return actor.FuelTank Is Nothing OrElse
                AvatarModel.FromActor(actor).Vessel.FuelPercent.Value > 0
        End Get
    End Property

    Private ReadOnly Property CanApproachStarVicinity As Boolean
        Get
            Return actor.Location.Place?.PlaceType = PlaceTypes.StarVicinity
        End Get
    End Property

    Private Sub ApproachStarVicinity()
        If CanApproachStarVicinity Then
            DoTurn()
            With actor.Location.Place
                SetLocation(RNG.FromEnumerable(.Map.Locations.Where(Function(x) x.Flags(.Identifier))))
            End With
        End If
    End Sub

    Private ReadOnly Property CanApproachPlanetVicinity As Boolean
        Get
            Return actor.Location.Place?.PlaceType = PlaceTypes.PlanetVicinity
        End Get
    End Property

    Private Sub ApproachPlanetVicinity()
        If CanApproachPlanetVicinity Then
            DoTurn()
            With actor.Location.Place
                SetLocation(RNG.FromEnumerable(.Map.Locations.Where(Function(x) x.Flags(.Identifier))))
            End With
        End If
    End Sub

    Private ReadOnly Property CanRefillFuel As Boolean
        Get
            Return actor.Location.Place?.PlaceType = PlaceTypes.Star
        End Get
    End Property

    Private Sub Refuel()
        If CanRefillFuel Then
            actor.FuelTank.CurrentValue = actor.FuelTank.MaximumValue.Value
        End If
    End Sub

    Private Sub RefillOxygen()
        If CanRefillOxygen Then
            actor.LifeSupport.CurrentValue = actor.LifeSupport.MaximumValue.Value
        End If
    End Sub

    Private ReadOnly Property CanRefillOxygen As Boolean
        Get
            Dim placeType = actor.Location.Place?.PlaceType
            If placeType <> PlaceTypes.Planet Then
                Return False
            End If
            Return PlanetTypes.Descriptors(actor.Location.Place.PlanetType).CanRefillOxygen
        End Get
    End Property

    Public ReadOnly Property Available As IEnumerable(Of (Text As String, VerbType As String)) Implements IAvatarVerbsModel.Available
        Get
            Return verbTable.
                Where(Function(x) VerbTypes.Descriptors(x.Key).Visible AndAlso x.Value.isAvailable.Invoke()).
                Select(Function(x) (VerbTypes.Descriptors(x.Key).Text, x.Key))
        End Get
    End Property

    Public ReadOnly Property HasAny As Boolean Implements IAvatarVerbsModel.HasAny
        Get
            Return Available.Any
        End Get
    End Property

    Public Sub Perform(verbType As String) Implements IAvatarVerbsModel.Perform
        If verbTable(verbType).isAvailable() Then
            verbTable(verbType).perform()
        End If
    End Sub

    Friend Shared Function FromActor(actor As IActor) As IAvatarVerbsModel
        Return New AvatarVerbsModel(actor)
    End Function

    Private ReadOnly Property CanEnterStarSystem As Boolean
        Get
            Return actor.Location.Place?.PlaceType = PlaceTypes.StarSystem
        End Get
    End Property

    Private ReadOnly Property CanEnterWormhole As Boolean
        Get
            Return actor.Location.Place?.PlaceType = PlaceTypes.Wormhole
        End Get
    End Property

    Private ReadOnly Property CanEnterOrbit As Boolean
        Get
            Return actor.Location.Place?.PlaceType = PlaceTypes.Planet OrElse actor.Location.Place?.PlaceType = PlaceTypes.Satellite
        End Get
    End Property

    Private Sub EnterStarSystem()
        If CanEnterStarSystem Then
            DoTurn()
            With actor.Location.Place
                SetLocation(RNG.FromEnumerable(.Map.Locations.Where(Function(x) x.Flags(.Identifier))))
            End With
        End If
    End Sub
    Private Sub EnterOrbit()
        If CanEnterOrbit Then
            DoTurn()
            With actor.Location.Place
                SetLocation(RNG.FromEnumerable(.Map.Locations.Where(Function(x) x.Flags(.Identifier))))
            End With
        End If
    End Sub
    Private Sub EnterWormhole()
        If CanEnterWormhole Then
            DoTurn()
            With actor.Location.Place
                SetLocation(.WormholeDestination)
            End With
        End If
    End Sub
End Class
