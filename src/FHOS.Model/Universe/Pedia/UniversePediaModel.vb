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

    Public ReadOnly Property Factions As IEnumerable(Of IGroupModel) Implements IUniversePediaModel.Factions
        Get
            Return SortedGroupsOfType(GroupTypes.Faction)
        End Get
    End Property

    Public ReadOnly Property StarSystems As IEnumerable(Of IGroupModel) Implements IUniversePediaModel.StarSystems
        Get
            Return SortedGroupsOfType(GroupTypes.StarSystem)
        End Get
    End Property

    Public ReadOnly Property PlanetVicinities As IEnumerable(Of IGroupModel) Implements IUniversePediaModel.PlanetVicinities
        Get
            Return SortedGroupsOfType(GroupTypes.PlanetVicinity)
        End Get
    End Property

    Public ReadOnly Property Satellites As IEnumerable(Of IGroupModel) Implements IUniversePediaModel.Satellites
        Get
            Return SortedGroupsOfType(GroupTypes.Satellite)
        End Get
    End Property
End Class
