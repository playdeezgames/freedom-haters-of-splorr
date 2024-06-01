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

    Public ReadOnly Property FactionList As IEnumerable(Of IGroupModel) Implements IUniversePediaModel.FactionList
        Get
            Return universe.Groups.Where(Function(x) x.EntityType = GroupTypes.Faction).Select(Function(x) GroupModel.FromGroup(x)).OrderBy(Function(x) x.Name)
        End Get
    End Property

    Public ReadOnly Property StarSystemList As IEnumerable(Of IGroupModel) Implements IUniversePediaModel.StarSystemList
        Get
            Return universe.Groups.Where(Function(x) x.EntityType = GroupTypes.StarSystem).Select(Function(x) GroupModel.FromGroup(x)).OrderBy(Function(x) x.Name)
        End Get
    End Property

    Public ReadOnly Property PlanetVicinityList As IEnumerable(Of IGroupModel) Implements IUniversePediaModel.PlanetVicinityList
        Get
            Return universe.Groups.Where(Function(x) x.EntityType = GroupTypes.PlanetVicinity).Select(Function(x) GroupModel.FromGroup(x)).OrderBy(Function(x) x.Name)
        End Get
    End Property

    Public ReadOnly Property SatelliteList As IEnumerable(Of IGroupModel) Implements IUniversePediaModel.SatelliteList
        Get
            Return universe.Groups.Where(Function(x) x.EntityType = GroupTypes.Satellite).Select(Function(x) GroupModel.FromGroup(x)).OrderBy(Function(x) x.Name)
        End Get
    End Property
End Class
