Imports FHOS.Persistence

Friend Class AvatarBioModel
    Inherits BaseAvatarModel
    Implements IAvatarBioModel

    Protected Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Public ReadOnly Property HomePlanet As IPlaceModel Implements IAvatarBioModel.HomePlanet
        Get
            Return PlaceModel.FromPlace(avatar.HomePlanet)
        End Get
    End Property

    Public ReadOnly Property Faction As IFactionModel Implements IAvatarBioModel.Faction
        Get
            Return FactionModel.FromFaction(avatar.Faction)
        End Get
    End Property

    Friend Shared Function FromAvatar(avatar As IActor) As IAvatarBioModel
        Return New AvatarBioModel(avatar)
    End Function
End Class
