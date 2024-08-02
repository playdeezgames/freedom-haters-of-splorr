Imports FHOS.Persistence

Friend Class AvatarBioModel
    Inherits BaseAvatarModel
    Implements IAvatarBioModel

    Protected Sub New(actor As IActor)
        MyBase.New(actor)
    End Sub

    Public ReadOnly Property HomePlanet As IGroupModel Implements IAvatarBioModel.HomePlanet
        Get
            Return GroupModel.FromGroup(actor.Yokes.Group(YokeTypes.HomePlanet))
        End Get
    End Property

    Public ReadOnly Property Faction As IGroupModel Implements IAvatarBioModel.Faction
        Get
            Return GroupModel.FromGroup(actor.Yokes.Group(YokeTypes.Faction))
        End Get
    End Property

    Friend Shared Function FromActor(actor As IActor) As IAvatarBioModel
        Return New AvatarBioModel(actor)
    End Function

    Public Function Reputation(group As IGroupModel) As Integer? Implements IAvatarBioModel.Reputation
        If group Is Nothing Then
            Throw New ArgumentNullException(NameOf(group))
        End If
        Return actor.GetReputation(CType(group, GroupModel).ToGroup)
    End Function
End Class
