Friend Class ActorKnownPlaces
    Inherits ActorDataClient
    Implements IActorKnownPlaces

    Protected Sub New(universeData As Data.UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Friend Shared Function FromId(universeData As Data.UniverseData, id As Integer) As IActorKnownPlaces
        Return New ActorKnownPlaces(universeData, id)
    End Function

    Public Sub LegacyAddKnownPlace(place As IPlace) Implements IActorKnownPlaces.LegacyAddKnownPlace
        If Not EntityData.Places.Discovered.ContainsKey(place.Id) Then
            EntityData.Places.Discovered(place.Id) = Universe.Turn
        End If
        EntityData.Places.Visited(place.Id) = Universe.Turn
    End Sub

    Public Function LegacyKnowsPlacesOfType(placeType As String) As Boolean Implements IActorKnownPlaces.LegacyKnowsPlacesOfType
        Return LegacyKnownPlaces.Any(Function(x) x.PlaceType = placeType)
    End Function

    Public Function LegacyGetKnownPlacesOfType(placeType As String) As IEnumerable(Of IPlace) Implements IActorKnownPlaces.LegacyGetKnownPlacesOfType
        Return LegacyKnownPlaces.Where(Function(x) x.PlaceType = placeType)
    End Function
    Public ReadOnly Property LegacyKnowsPlaces As Boolean Implements IActorKnownPlaces.LegacyKnowsPlaces
        Get
            Return EntityData.Places.Discovered.Any
        End Get
    End Property

    Public ReadOnly Property LegacyKnownPlaces As IEnumerable(Of IPlace) Implements IActorKnownPlaces.LegacyKnownPlaces
        Get
            Return EntityData.Places.Discovered.Select(Function(x) Place.FromId(UniverseData, x.Key)).OrderBy(Function(x) x.Name)
        End Get
    End Property
End Class
