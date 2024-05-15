Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class AvatarModel
    Inherits BaseAvatarModel
    Implements IAvatarModel

    Protected Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Friend Shared Function FromActor(actor As IActor) As IAvatarModel
        If actor Is Nothing Then
            Return Nothing
        End If
        Return New AvatarModel(actor)
    End Function

    Public ReadOnly Property X As Integer Implements IAvatarModel.X
        Get
            Return avatar.Location.Column
        End Get
    End Property

    Public ReadOnly Property Y As Integer Implements IAvatarModel.Y
        Get
            Return avatar.Location.Row
        End Get
    End Property

    Public ReadOnly Property MapName As String Implements IAvatarModel.MapName
        Get
            Return avatar.Location.Map.Name
        End Get
    End Property

    Public ReadOnly Property OxygenHue As Integer Implements IAvatarModel.OxygenHue
        Get
            If OxygenPercent <= 33 Then
                Return Hues.Red
            End If
            If OxygenPercent <= 66 Then
                Return Hues.Yellow
            End If
            Return Hues.Green
        End Get
    End Property

    Public ReadOnly Property OxygenPercent As Integer Implements IAvatarModel.OxygenPercent
        Get
            Return avatar.LifeSupport.CurrentValue * 100 \ avatar.LifeSupport.MaximumValue.Value
        End Get
    End Property

    Public ReadOnly Property HasVerbs As Boolean Implements IAvatarModel.HasVerbs
        Get
            Return AvailableVerbs.Any OrElse
                KnowsPlaces
        End Get
    End Property

    Public ReadOnly Property Tutorial As IAvatarTutorialModel Implements IAvatarModel.Tutorial
        Get
            Return New AvatarTutorialModel(avatar)
        End Get
    End Property

    Public ReadOnly Property FuelPercent As Integer Implements IAvatarModel.FuelPercent
        Get
            Return avatar.Fuel * 100 \ avatar.MaximumFuel
        End Get
    End Property

    Public ReadOnly Property FuelHue As Integer Implements IAvatarModel.FuelHue
        Get
            If FuelPercent <= 33 Then
                Return Hues.Red
            End If
            If FuelPercent <= 66 Then
                Return Hues.Yellow
            End If
            Return Hues.Green
        End Get
    End Property

    Public ReadOnly Property CanMove As Boolean Implements IAvatarModel.CanMove
        Get
            Return FuelPercent > 0
        End Get
    End Property

    Private ReadOnly Property HasCrew As Boolean
        Get
            Return avatar.HasCrew
        End Get
    End Property

    Private ReadOnly Property HasVessel As Boolean
        Get
            Return avatar.Vessel IsNot Nothing
        End Get
    End Property

    Public ReadOnly Property IsGameOver As Boolean Implements IAvatarModel.IsGameOver
        Get
            Return IsDead OrElse IsBankrupt
        End Get
    End Property

    Public ReadOnly Property Turn As Integer Implements IAvatarModel.Turn
        Get
            Return avatar.Turn
        End Get
    End Property

    Public ReadOnly Property Jools As Integer Implements IAvatarModel.Jools
        Get
            Return avatar.Jools
        End Get
    End Property

    Public ReadOnly Property MinimumJools As Integer Implements IAvatarModel.MinimumJools
        Get
            Return avatar.MinimumJools
        End Get
    End Property

    Public ReadOnly Property IsDead As Boolean Implements IAvatarModel.IsDead
        Get
            Return avatar.LifeSupport.CurrentValue = avatar.LifeSupport.MinimumValue.Value
        End Get
    End Property

    Public ReadOnly Property IsBankrupt As Boolean Implements IAvatarModel.IsBankrupt
        Get
            Return avatar.Jools < avatar.MinimumJools
        End Get
    End Property

    Public ReadOnly Property KnowsPlaces As Boolean Implements IAvatarModel.KnowsPlaces
        Get
            Return avatar.KnowsPlaces
        End Get
    End Property

    Public ReadOnly Property CurrentPlace As IPlaceModel Implements IAvatarModel.CurrentPlace
        Get
            If avatar.Location.Place IsNot Nothing Then
                Return PlaceModel.FromPlace(avatar.Location.Place)
            End If
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property AvailableVerbs As IEnumerable(Of (Text As String, VerbType As String)) Implements IAvatarModel.AvailableVerbs
        Get
            Return verbTable.
                Where(Function(x) VerbTypes.Descriptors(x.Key).Visible AndAlso x.Value.isAvailable.Invoke()).
                Select(Function(x) (VerbTypes.Descriptors(x.Key).Text, x.Key))
        End Get
    End Property

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

    Public Sub DoVerb(verbType As String) Implements IAvatarModel.DoVerb
        If verbTable(verbType).isAvailable() Then
            verbTable(verbType).perform()
        End If
    End Sub

    Private ReadOnly verbTable As IReadOnlyDictionary(Of String, (isAvailable As Func(Of Boolean), perform As Action)) =
        New Dictionary(Of String, (isAvailable As Func(Of Boolean), perform As Action)) From
        {
            {VerbTypes.Status, (Function() True, Sub() Return)},
            {VerbTypes.KnownPlaces, (Function() KnowsPlaces, Sub() Return)},
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
            {VerbTypes.Crew, (Function() HasCrew, Sub() Return)},
            {VerbTypes.Vessel, (Function() HasVessel, Sub() Return)}
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

    Private ReadOnly Property CanRefillOxygen As Boolean
        Get
            Dim placeType = avatar.Location.Place?.PlaceType
            If placeType <> PlaceTypes.Planet Then
                Return False
            End If
            Return PlanetTypes.Descriptors(avatar.Location.Place.PlanetType).CanRefillOxygen
        End Get
    End Property

    Public ReadOnly Property HomePlanet As IPlaceModel Implements IAvatarModel.HomePlanet
        Get
            Return PlaceModel.FromPlace(avatar.HomePlanet)
        End Get
    End Property

    Public ReadOnly Property Faction As IFactionModel Implements IAvatarModel.Faction
        Get
            Return FactionModel.FromFaction(avatar.Faction)
        End Get
    End Property

    Public ReadOnly Property IsInteracting As Boolean Implements IAvatarModel.IsInteracting
        Get
            Return avatar.Interactor IsNot Nothing
        End Get
    End Property

    Public ReadOnly Property AvailableCrew As IEnumerable(Of (Name As String, Actor As IActorModel)) Implements IAvatarModel.AvailableCrew
        Get
            Return avatar.Crew.Select(Function(x) (x.Name, ActorModel.FromActor(x)))
        End Get
    End Property

    Public ReadOnly Property Position As (X As Integer, Y As Integer) Implements IAvatarModel.Position
        Get
            Return (X, Y)
        End Get
    End Property

    Private Sub RefillOxygen()
        If CanRefillOxygen Then
            avatar.LifeSupport.CurrentValue = avatar.LifeSupport.MaximumValue.Value
        End If
    End Sub

    Public Function KnowsPlacesOfType(placeType As String) As Boolean Implements IAvatarModel.KnowsPlacesOfType
        Return avatar.KnowsPlacesOfType(placeType)
    End Function

    Public Function GetKnownPlacesOfType(placeType As String) As IEnumerable(Of (Text As String, Item As IPlaceModel)) Implements IAvatarModel.GetKnownPlacesOfType
        Return avatar.GetKnownPlacesOfType(placeType).Select(Function(x) (x.Name, PlaceModel.FromPlace(x)))
    End Function

    Public Sub LeaveInteraction() Implements IAvatarModel.LeaveInteraction
        avatar.Interactor = Nothing
    End Sub

    Public Sub Push(actor As IActorModel) Implements IAvatarModel.Push
        avatar.Universe.PushAvatar(ActorModel.GetActor(actor))
    End Sub

    Public Sub Pop() Implements IAvatarModel.Pop
        avatar.Universe.PopAvatar()
    End Sub
End Class
