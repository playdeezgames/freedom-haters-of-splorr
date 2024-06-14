Imports FHOS.Persistence

Friend Class GroupChildrenModel
    Implements IGroupChildrenModel

    Private ReadOnly group As IGroup

    Public Sub New(group As IGroup)
        Me.group = group
    End Sub

    Private Function SortedChildrenOfType(groupType As String) As IEnumerable(Of IGroupModel)
        Return group.ChildrenOfType(groupType).Select(AddressOf GroupModel.FromGroup).OrderBy(Function(x) x.Name)
    End Function

    Public ReadOnly Property ChildPlanets As IEnumerable(Of IGroupModel) Implements IGroupChildrenModel.ChildPlanets
        Get
            Return SortedChildrenOfType(GroupTypes.PlanetVicinity)
        End Get
    End Property

    Public ReadOnly Property ChildSatellites As IEnumerable(Of IGroupModel) Implements IGroupChildrenModel.ChildSatellites
        Get
            Return SortedChildrenOfType(GroupTypes.Satellite)
        End Get
    End Property

    Public ReadOnly Property ChildPlanetFactions As IEnumerable(Of IGroupModel) Implements IGroupChildrenModel.ChildPlanetFactions
        Get
            Dim planetVicinities = group.ChildrenOfType(GroupTypes.PlanetVicinity)
            Return planetVicinities.
                Select(Function(x) x.SingleParent(GroupTypes.Faction)).
                GroupBy(Function(z) z.Id).
                Select(Function(w) w.First).
                Select(AddressOf GroupModel.FromGroup)
        End Get
    End Property

    Friend Shared Function FromGroup(group As IGroup) As IGroupChildrenModel
        Return New GroupChildrenModel(group)
    End Function
End Class
