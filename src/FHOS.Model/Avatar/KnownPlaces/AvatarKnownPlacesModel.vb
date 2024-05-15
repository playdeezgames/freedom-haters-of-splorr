Imports FHOS.Persistence

Friend Class AvatarKnownPlacesModel
    Inherits BaseAvatarModel
    Implements IAvatarKnownPlacesModel

    Protected Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Friend Shared Function FromActor(avatar As IActor) As IAvatarKnownPlacesModel
        Return New AvatarKnownPlacesModel(avatar)
    End Function

    Public Function HasAnyOfType(placeType As String) As Boolean Implements IAvatarKnownPlacesModel.HasAnyOfType
        Return avatar.KnowsPlacesOfType(placeType)
    End Function

    Public Function OfType(placeType As String) As IEnumerable(Of (Text As String, Item As IPlaceModel)) Implements IAvatarKnownPlacesModel.OfType
        Return avatar.GetKnownPlacesOfType(placeType).Select(Function(x) (x.Name, PlaceModel.FromPlace(x)))
    End Function
End Class
