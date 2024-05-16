Imports FHOS.Persistence

Friend Class AvatarKnownPlacesModel
    Inherits BaseAvatarModel
    Implements IAvatarKnownPlacesModel

    Protected Sub New(actor As IActor)
        MyBase.New(actor)
    End Sub

    Friend Shared Function FromActor(actor As IActor) As IAvatarKnownPlacesModel
        Return New AvatarKnownPlacesModel(actor)
    End Function

    Public Function HasAnyOfType(placeType As String) As Boolean Implements IAvatarKnownPlacesModel.HasAnyOfType
        Return actor.KnowsPlacesOfType(placeType)
    End Function

    Public Function OfType(placeType As String) As IEnumerable(Of (Text As String, Item As IPlaceModel)) Implements IAvatarKnownPlacesModel.OfType
        Return actor.GetKnownPlacesOfType(placeType).Select(Function(x) (x.Name, PlaceModel.FromPlace(x)))
    End Function
End Class
