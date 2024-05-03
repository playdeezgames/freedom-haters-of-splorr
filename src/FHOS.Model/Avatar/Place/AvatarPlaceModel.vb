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

    Public ReadOnly Property CanEnterStarSystem As Boolean Implements IAvatarPlaceModel.CanEnterStarSystem
        Get
            Return avatar.Location.Place?.PlaceType = PlaceTypes.StarSystem
        End Get
    End Property

    Public Sub EnterStarSystem() Implements IAvatarPlaceModel.EnterStarSystem
        If CanEnterStarSystem Then
            DoTurn()
            With avatar.Location.Place
                SetLocation(RNG.FromEnumerable(.Map.Locations.Where(Function(x) x.HasFlag(.Identifier))))
            End With
        End If
    End Sub

    Public ReadOnly Property CanApproachStarVicinity As Boolean Implements IAvatarPlaceModel.CanApproachStarVicinity
        Get
            Return avatar.Location.Place?.PlaceType = PlaceTypes.StarVicinity
        End Get
    End Property

    Public Sub ApproachStarVicinity() Implements IAvatarPlaceModel.ApproachStarVicinity
        If CanApproachStarVicinity Then
            DoTurn()
            With avatar.Location.Place
                SetLocation(RNG.FromEnumerable(.Map.Locations.Where(Function(x) x.HasFlag(.Identifier))))
            End With
        End If
    End Sub
    Public ReadOnly Property CanRefillFuel As Boolean Implements IAvatarPlaceModel.CanRefillFuel
        Get
            Return avatar.Location.Place?.PlaceType = PlaceTypes.Star
        End Get
    End Property

    Public Sub Refuel() Implements IAvatarPlaceModel.Refuel
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
            Return PlanetTypes.Descriptors(placeType).CanRefillOxygen
        End Get
    End Property

    Public Sub RefillOxygen() Implements IAvatarPlaceModel.RefillOxygen
        If CanRefillOxygen Then
            avatar.Oxygen = avatar.MaximumOxygen
        End If
    End Sub
End Class
