Friend Class ActorKnownPlaces
    Inherits ActorDataClient
    Implements IActorKnownPlaces

    Protected Sub New(universeData As Data.UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Friend Shared Function FromId(universeData As Data.UniverseData, id As Integer) As IActorKnownPlaces
        Return New ActorKnownPlaces(universeData, id)
    End Function

    Public Sub Add(place As IPlace) Implements IActorKnownPlaces.Add
        If Not EntityData.Places.Discovered.ContainsKey(place.Id) Then
            EntityData.Places.Discovered(place.Id) = Universe.Turn
        End If
        EntityData.Places.Visited(place.Id) = Universe.Turn
    End Sub

    Public Function HasPlacesOfType(placeType As String) As Boolean Implements IActorKnownPlaces.HasPlacesOfType
        Return All.Any(Function(x) x.PlaceType = placeType)
    End Function

    Public Function PlacesOfType(placeType As String) As IEnumerable(Of IPlace) Implements IActorKnownPlaces.PlacesOfType
        Return All.Where(Function(x) x.PlaceType = placeType)
    End Function
    Public ReadOnly Property HasAny As Boolean Implements IActorKnownPlaces.HasAny
        Get
            Return EntityData.Places.Discovered.Any
        End Get
    End Property

    Public ReadOnly Property All As IEnumerable(Of IPlace) Implements IActorKnownPlaces.All
        Get
            Return EntityData.Places.Discovered.Select(Function(x) Place.FromId(UniverseData, x.Key)).OrderBy(Function(x) x.Name)
        End Get
    End Property
End Class
