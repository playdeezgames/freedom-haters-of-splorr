Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class AvatarPlaceModel
    Inherits BaseAvatarModel
    Implements IAvatarPlaceModel

    Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Public ReadOnly Property Current As IPlaceModel Implements IAvatarPlaceModel.Current
        Get
            If avatar.Location.Place IsNot Nothing Then
                Return New PlaceModel(avatar.Location.Place)
            End If
            Return Nothing
        End Get
    End Property

    Private ReadOnly Property CanEnterStarSystem As Boolean
        Get
            Return avatar.Location.Place?.PlaceType = PlaceTypes.StarSystem
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

    Public ReadOnly Property CanApproachPlanetVicinity As Boolean Implements IAvatarPlaceModel.CanApproachPlanetVicinity
        Get
            Return avatar.Location.Place?.PlaceType = PlaceTypes.PlanetVicinity
        End Get
    End Property

    Public Sub ApproachPlanetVicinity() Implements IAvatarPlaceModel.ApproachPlanetVicinity
        If CanApproachPlanetVicinity Then
            DoTurn()
            With avatar.Location.Place
                SetLocation(RNG.FromEnumerable(.Map.Locations.Where(Function(x) x.HasFlag(.Identifier))))
            End With
        End If
    End Sub

    Public ReadOnly Property CanRefillOxygen As Boolean Implements IAvatarPlaceModel.CanRefillOxygen
        Get
            Dim placeType = avatar.Location.Place?.PlaceType
            If placeType <> PlaceTypes.Planet Then
                Return False
            End If
            Return PlanetTypes.Descriptors(avatar.Location.Place.PlanetType).CanRefillOxygen
        End Get
    End Property

    Private ReadOnly verbTable As IReadOnlyDictionary(Of String, (isAvailable As Func(Of Boolean), perform As Action)) =
        New Dictionary(Of String, (isAvailable As Func(Of Boolean), perform As Action)) From
        {
            {VerbTypes.RefillOxygen, (Function() CanRefillOxygen, AddressOf RefillOxygen)},
            {VerbTypes.Refuel, (Function() CanRefillFuel, AddressOf Refuel)},
            {VerbTypes.EnterStarSystem, (Function() CanEnterStarSystem, AddressOf EnterStarSystem)},
            {VerbTypes.ApproachPlanetVicinity, (Function() CanApproachPlanetVicinity, AddressOf ApproachPlanetVicinity)},
            {VerbTypes.ApproachStarVicinity, (Function() CanApproachStarVicinity, AddressOf ApproachStarVicinity)}
        }

    Public ReadOnly Property Verbs As IEnumerable(Of String) Implements IAvatarPlaceModel.Verbs
        Get
            Return verbTable.Where(Function(x) x.Value.isAvailable.Invoke()).Select(Function(x) x.Key)
        End Get
    End Property

    Public Sub RefillOxygen() Implements IAvatarPlaceModel.RefillOxygen
        If CanRefillOxygen Then
            avatar.Oxygen = avatar.MaximumOxygen
        End If
    End Sub

    Public Sub DoVerb(verbType As String) Implements IAvatarPlaceModel.DoVerb
        If verbTable(verbType).isAvailable() Then
            verbTable(verbType).perform()
        End If
    End Sub
End Class
