Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class AvatarVerbsModel
    Inherits BaseAvatarModel
    Implements IAvatarVerbsModel

    Protected Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Private ReadOnly verbTable As IReadOnlyDictionary(Of String, (isAvailable As Func(Of Boolean), perform As Action)) =
        New Dictionary(Of String, (isAvailable As Func(Of Boolean), perform As Action)) From
        {
            {VerbTypes.Status, (Function() True, Sub() Return)},
            {VerbTypes.KnownPlaces, (Function() avatar.KnowsPlaces, Sub() Return)},
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
            {VerbTypes.Crew, (Function() avatar.HasCrew, Sub() Return)},
            {VerbTypes.Vessel, (Function() avatar.Vessel IsNot Nothing, Sub() Return)}
        }

    Private Sub Move(facing As Integer)
        avatar.Facing = facing
        Dim delta = Persistence.Facing.Deltas(facing)
        If Not CanMove Then
            Return
        End If
        DoTurn()
        DoFuelConsumption()
        Dim location = avatar.Location
        Dim nextColumn = location.Column + delta.X
        Dim nextRow = location.Row + delta.Y
        Dim map = location.Map
        Dim nextLocation = map.GetLocation(nextColumn, nextRow)
        If nextLocation Is Nothing Then
            Return
        End If
        If nextLocation.Actor IsNot Nothing Then
            avatar.Interactor = nextLocation.Actor
            Return
        End If
        If nextLocation.HasTargetLocation Then
            nextLocation = nextLocation.TargetLocation
        End If
        SetLocation(nextLocation)
    End Sub

    Private Sub DoFuelConsumption()
        If avatar.ConsumesFuel Then
            avatar.Fuel -= 1
            If Not avatar.HasFuel Then
                avatar.TriggerTutorial(TutorialTypes.OutOfFuel)
            End If
        End If
    End Sub

    Private Sub DistressSignal()
        Dim fuelAdded = avatar.MaximumFuel - avatar.Fuel
        Dim fuelPrice = 1 'TODO: don't just pick a magic number!
        Dim price = fuelPrice * fuelAdded
        avatar.Fuel = avatar.MaximumFuel
        avatar.Jools -= fuelAdded * fuelPrice
        avatar.AddMessage(
            "Emergency Refuel",
            ($"Added {fuelAdded} fuel!", Hues.Black),
            ($"Price {price} jools!", Hues.Black))
    End Sub

    Private ReadOnly Property CanMove As Boolean
        Get
            Return Not avatar.ConsumesFuel OrElse AvatarModel.FromActor(avatar).FuelPercent > 0
        End Get
    End Property

    Private ReadOnly Property CanApproachStarVicinity As Boolean
        Get
            Return avatar.Location.Place?.PlaceType = PlaceTypes.StarVicinity
        End Get
    End Property

    Private Sub ApproachStarVicinity()
        If CanApproachStarVicinity Then
            DoTurn()
            With avatar.Location.Place
                SetLocation(RNG.FromEnumerable(.Map.Locations.Where(Function(x) x.HasFlag(.Identifier))))
            End With
        End If
    End Sub

    Private ReadOnly Property CanApproachPlanetVicinity As Boolean
        Get
            Return avatar.Location.Place?.PlaceType = PlaceTypes.PlanetVicinity
        End Get
    End Property

    Private Sub ApproachPlanetVicinity()
        If CanApproachPlanetVicinity Then
            DoTurn()
            With avatar.Location.Place
                SetLocation(RNG.FromEnumerable(.Map.Locations.Where(Function(x) x.HasFlag(.Identifier))))
            End With
        End If
    End Sub

    Private ReadOnly Property CanRefillFuel As Boolean
        Get
            Return avatar.Location.Place?.PlaceType = PlaceTypes.Star
        End Get
    End Property

    Private Sub Refuel()
        If CanRefillFuel Then
            avatar.Fuel = avatar.MaximumFuel
        End If
    End Sub

    Private Sub RefillOxygen()
        If CanRefillOxygen Then
            avatar.LifeSupport.CurrentValue = avatar.LifeSupport.MaximumValue.Value
        End If
    End Sub

    Private ReadOnly Property CanRefillOxygen As Boolean
        Get
            Dim placeType = avatar.Location.Place?.PlaceType
            If placeType <> PlaceTypes.Planet Then
                Return False
            End If
            Return PlanetTypes.Descriptors(avatar.Location.Place.PlanetType).CanRefillOxygen
        End Get
    End Property

    Public ReadOnly Property AvailableVerbs As IEnumerable(Of (Text As String, VerbType As String)) Implements IAvatarVerbsModel.AvailableVerbs
        Get
            Return verbTable.
                Where(Function(x) VerbTypes.Descriptors(x.Key).Visible AndAlso x.Value.isAvailable.Invoke()).
                Select(Function(x) (VerbTypes.Descriptors(x.Key).Text, x.Key))
        End Get
    End Property

    Public ReadOnly Property HasVerbs As Boolean Implements IAvatarVerbsModel.HasVerbs
        Get
            Return AvailableVerbs.Any
        End Get
    End Property

    Public Sub DoVerb(verbType As String) Implements IAvatarVerbsModel.DoVerb
        If verbTable(verbType).isAvailable() Then
            verbTable(verbType).perform()
        End If
    End Sub

    Friend Shared Function FromAvatar(avatar As IActor) As IAvatarVerbsModel
        Return New AvatarVerbsModel(avatar)
    End Function

    Private ReadOnly Property CanEnterStarSystem As Boolean
        Get
            Return avatar.Location.Place?.PlaceType = PlaceTypes.StarSystem
        End Get
    End Property

    Private ReadOnly Property CanEnterWormhole As Boolean
        Get
            Return avatar.Location.Place?.PlaceType = PlaceTypes.Wormhole
        End Get
    End Property

    Private ReadOnly Property CanEnterOrbit As Boolean
        Get
            Return avatar.Location.Place?.PlaceType = PlaceTypes.Planet OrElse avatar.Location.Place?.PlaceType = PlaceTypes.Satellite
        End Get
    End Property

    Private Sub EnterStarSystem()
        If CanEnterStarSystem Then
            DoTurn()
            With avatar.Location.Place
                SetLocation(RNG.FromEnumerable(.Map.Locations.Where(Function(x) x.HasFlag(.Identifier))))
            End With
        End If
    End Sub
    Private Sub EnterOrbit()
        If CanEnterOrbit Then
            DoTurn()
            With avatar.Location.Place
                SetLocation(RNG.FromEnumerable(.Map.Locations.Where(Function(x) x.HasFlag(.Identifier))))
            End With
        End If
    End Sub
    Private Sub EnterWormhole()
        If CanEnterWormhole Then
            DoTurn()
            With avatar.Location.Place
                SetLocation(.WormholeDestination)
            End With
        End If
    End Sub
End Class
