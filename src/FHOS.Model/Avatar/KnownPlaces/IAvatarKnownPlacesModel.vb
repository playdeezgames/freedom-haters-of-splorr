Public Interface IAvatarKnownPlacesModel
    Function HasAnyOfType(placeType As String) As Boolean
    Function OfType(placeType As String) As IEnumerable(Of (Text As String, Item As IPlaceModel))
End Interface
