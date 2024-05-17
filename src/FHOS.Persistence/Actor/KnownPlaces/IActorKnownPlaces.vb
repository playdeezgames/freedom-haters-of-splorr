Public Interface IActorKnownPlaces
    ReadOnly Property HasAny As Boolean
    ReadOnly Property All As IEnumerable(Of IPlace)
    Function HasPlacesOfType(placeType As String) As Boolean
    Function PlacesOfType(placeType As String) As IEnumerable(Of IPlace)
    Sub Add(place As IPlace)
End Interface
