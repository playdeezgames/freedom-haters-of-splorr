Imports FHOS.Persistence

Friend Class UniversePediaModel
    Implements IUniversePediaModel

    Private ReadOnly universe As IUniverse

    Public Sub New(universe As IUniverse)
        Me.universe = universe
    End Sub

    Friend Shared Function FromUniverse(universe As IUniverse) As IUniversePediaModel
        Return New UniversePediaModel(universe)
    End Function

    Private Function SortedGroupsOfType(groupType As String) As IEnumerable(Of IGroupModel)
        Return universe.Groups.Where(Function(x) x.EntityType = groupType).Select(Function(x) GroupModel.FromGroup(x)).OrderBy(Function(x) x.Name)
    End Function

    Public ReadOnly Property FactionList As IEnumerable(Of IGroupModel) Implements IUniversePediaModel.FactionList
        Get
            Return SortedGroupsOfType(GroupTypes.Faction)
        End Get
    End Property

    Public ReadOnly Property StarSystemList As IEnumerable(Of IGroupModel) Implements IUniversePediaModel.StarSystemList
        Get
            Return SortedGroupsOfType(GroupTypes.StarSystem)
        End Get
    End Property

    Public ReadOnly Property PlanetVicinityList As IEnumerable(Of IGroupModel) Implements IUniversePediaModel.PlanetVicinityList
        Get
            Return SortedGroupsOfType(GroupTypes.PlanetVicinity)
        End Get
    End Property

    Public ReadOnly Property SatelliteList As IEnumerable(Of IGroupModel) Implements IUniversePediaModel.SatelliteList
        Get
            Return SortedGroupsOfType(GroupTypes.Satellite)
        End Get
    End Property
End Class
