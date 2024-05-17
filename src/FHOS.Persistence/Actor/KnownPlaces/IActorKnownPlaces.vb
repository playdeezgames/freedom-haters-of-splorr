Public Interface IActorKnownPlaces
    ReadOnly Property LegacyKnowsPlaces As Boolean
    ReadOnly Property LegacyKnownPlaces As IEnumerable(Of IPlace)
    Function LegacyKnowsPlacesOfType(placeType As String) As Boolean
    Function LegacyGetKnownPlacesOfType(placeType As String) As IEnumerable(Of IPlace)
    Sub LegacyAddKnownPlace(place As IPlace)
End Interface
