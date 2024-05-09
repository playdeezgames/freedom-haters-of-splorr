Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class AvatarModel
    Inherits BaseAvatarModel
    Implements IAvatarModel

    Public Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

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
                Return Hue.Red
            End If
            If OxygenPercent <= 66 Then
                Return Hue.Yellow
            End If
            Return Hue.Green
        End Get
    End Property

    Public ReadOnly Property OxygenPercent As Integer Implements IAvatarModel.OxygenPercent
        Get
            Return avatar.Oxygen * 100 \ avatar.MaximumOxygen
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
                Return Hue.Red
            End If
            If FuelPercent <= 66 Then
                Return Hue.Yellow
            End If
            Return Hue.Green
        End Get
    End Property

    Public ReadOnly Property CanMove As Boolean Implements IAvatarModel.CanMove
        Get
            Return FuelPercent > 0
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
            Return avatar.Oxygen = 0
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
                Return New PlaceModel(avatar.Location.Place)
            End If
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property AvailableVerbs As IEnumerable(Of String) Implements IAvatarModel.AvailableVerbs
        Get
            Return verbTable.Where(Function(x) x.Value.isAvailable.Invoke()).Select(Function(x) x.Key)
        End Get
    End Property

    Public Sub DoDistressSignal() Implements IAvatarModel.DoDistressSignal
        Dim fuelAdded = avatar.MaximumFuel - avatar.Fuel
        Dim fuelPrice = 1 'TODO: don't just pick a magic number!
        Dim price = fuelPrice * fuelAdded
        avatar.Fuel = avatar.MaximumFuel
        avatar.Jools -= fuelAdded * fuelPrice
        avatar.AddMessage(
            "Emergency Refuel",
            ($"Added {fuelAdded} fuel!", Hue.Black),
            ($"Price {price} jools!", Hue.Black))
    End Sub

    Public Sub DoVerb(verbType As String) Implements IAvatarModel.DoVerb
        If verbTable(verbType).isAvailable() Then
            verbTable(verbType).perform()
        End If
    End Sub

    Private ReadOnly verbTable As IReadOnlyDictionary(Of String, (isAvailable As Func(Of Boolean), perform As Action)) =
        New Dictionary(Of String, (isAvailable As Func(Of Boolean), perform As Action)) From
        {
            {VerbTypes.KnownPlaces, (Function() KnowsPlaces, Sub() Return)},
            {VerbTypes.RefillOxygen, (Function() CanRefillOxygen, AddressOf RefillOxygen)},
            {VerbTypes.Refuel, (Function() CanRefillFuel, AddressOf Refuel)},
            {VerbTypes.EnterWormhole, (Function() CanEnterWormhole, AddressOf EnterWormhole)},
            {VerbTypes.EnterOrbit, (Function() CanEnterOrbit, AddressOf EnterOrbit)},
            {VerbTypes.EnterStarSystem, (Function() CanEnterStarSystem, AddressOf EnterStarSystem)},
            {VerbTypes.ApproachPlanetVicinity, (Function() CanApproachPlanetVicinity, AddressOf ApproachPlanetVicinity)},
            {VerbTypes.ApproachStarVicinity, (Function() CanApproachStarVicinity, AddressOf ApproachStarVicinity)},
            {VerbTypes.DistressSignal, (Function() Not CanMove, AddressOf DoDistressSignal)}
        }

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
    Private Sub RefillOxygen()
        If CanRefillOxygen Then
            avatar.Oxygen = avatar.MaximumOxygen
        End If
    End Sub

    Public Sub Move(facing As Integer, delta As (X As Integer, Y As Integer)) Implements IAvatarModel.Move
        avatar.Facing = facing
        If Not CanMove Then
            Return
        End If
        DoTurn()
        avatar.Fuel -= 1
        If Not avatar.HasFuel Then
            avatar.TriggerTutorial(TutorialTypes.OutOfFuel)
        End If
        Dim location = avatar.Location
        Dim nextColumn = location.Column + delta.X
        Dim nextRow = location.Row + delta.Y
        Dim map = location.Map
        Dim nextLocation = map.GetLocation(nextColumn, nextRow)
        If nextLocation IsNot Nothing Then
            If nextLocation.HasTargetLocation Then
                SetLocation(nextLocation.TargetLocation)
            Else
                SetLocation(nextLocation)
            End If
        End If
    End Sub

    Public Function KnowsPlacesOfType(placeType As String) As Boolean Implements IAvatarModel.KnowsPlacesOfType
        Return avatar.KnowsPlacesOfType(placeType)
    End Function

    Public Function GetKnownPlacesOfType(placeType As String) As IEnumerable(Of (Text As String, Item As IPlaceModel)) Implements IAvatarModel.GetKnownPlacesOfType
        Return avatar.GetKnownPlacesOfType(placeType).Select(Function(x) (x.Name, CType(New PlaceModel(x), IPlaceModel)))
    End Function
End Class
