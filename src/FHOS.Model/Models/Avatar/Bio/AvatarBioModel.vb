Imports FHOS.Persistence

Friend Class AvatarBioModel
    Inherits BaseAvatarModel
    Implements IAvatarBioModel

    Protected Sub New(actor As IActor)
        MyBase.New(actor)
    End Sub

    Public ReadOnly Property HomePlanet As IGroupModel Implements IAvatarBioModel.HomePlanet
        Get
            Return GroupModel.FromGroup(actor.Yokes.YokedGroup(YokeTypes.HomePlanet))
        End Get
    End Property

    Public ReadOnly Property Group As IGroupModel Implements IAvatarBioModel.Group
        Get
            Return GroupModel.FromGroup(actor.Yokes.YokedGroup(YokeTypes.Faction))
        End Get
    End Property

    Friend Shared Function FromActor(actor As IActor) As IAvatarBioModel
        Return New AvatarBioModel(actor)
    End Function
End Class
