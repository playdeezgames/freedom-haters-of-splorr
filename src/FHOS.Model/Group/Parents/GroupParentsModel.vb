Imports FHOS.Persistence

Friend Class GroupParentsModel
    Implements IGroupParentsModel

    Private ReadOnly group As IGroup

    Public Sub New(group As IGroup)
        Me.group = group
    End Sub

    Public ReadOnly Property ParentStarSystem As IGroupModel Implements IGroupParentsModel.ParentStarSystem
        Get
            Return OptionalSingleParent(GroupTypes.StarSystem)
        End Get
    End Property

    Public ReadOnly Property ParentPlanet As IGroupModel Implements IGroupParentsModel.ParentPlanet
        Get
            Return OptionalSingleParent(GroupTypes.PlanetVicinity)
        End Get
    End Property

    Private Function OptionalSingleParent(groupType As String) As IGroupModel
        Return GroupModel.FromGroup(group.Parents.SingleOrDefault(Function(x) x.EntityType = groupType))
    End Function

    Public ReadOnly Property ParentFaction As IGroupModel Implements IGroupParentsModel.ParentFaction
        Get
            Return OptionalSingleParent(GroupTypes.Faction)
        End Get
    End Property

    Friend Shared Function FromGroup(group As IGroup) As IGroupParentsModel
        Return New GroupParentsModel(group)
    End Function
End Class
