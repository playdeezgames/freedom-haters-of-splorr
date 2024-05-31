Imports FHOS.Data

Friend Class Group
    Inherits GroupDataClient
    Implements IGroup
    Friend Shared Function FromId(universeData As UniverseData, id As Integer?) As IGroup
        Return If(id.HasValue, New Group(universeData, id.Value), Nothing)
    End Function

    Protected Sub New(universeData As Data.UniverseData, factionId As Integer)
        MyBase.New(universeData, factionId)
    End Sub

    Public Sub AddParent(parent As IGroup) Implements IGroup.AddParent
        If Not EntityData.Parents.Contains(parent.Id) Then
            EntityData.Parents.Add(parent.Id)
            CType(parent, Group).EntityData.Children.Add(Id)
        End If
    End Sub

    Public Sub RemoveParent(parent As IGroup) Implements IGroup.RemoveParent
        If parent IsNot Nothing Then
            If EntityData.Parents.Contains(parent.Id) Then
                EntityData.Parents.Remove(parent.Id)
                CType(parent, Group).EntityData.Children.Remove(Id)
            End If
        End If
    End Sub

    Public Property MinimumPlanetCount As Integer Implements IGroup.MinimumPlanetCount
        Get
            Return GetStatistic(StatisticTypes.MinimumPlanetCount).Value
        End Get
        Set(value As Integer)
            SetStatistic(StatisticTypes.MinimumPlanetCount, value)
        End Set
    End Property

    Public ReadOnly Property Name As String Implements IGroup.Name
        Get
            Return EntityData.Metadatas(MetadataTypes.Name)
        End Get
    End Property

    Public Property PlanetCount As Integer Implements IGroup.PlanetCount
        Get
            Return If(GetStatistic(StatisticTypes.PlanetCount), 0)
        End Get
        Set(value As Integer)
            SetStatistic(StatisticTypes.PlanetCount, value)
        End Set
    End Property

    Public Property SatelliteCount As Integer Implements IGroup.SatelliteCount
        Get
            Return If(GetStatistic(StatisticTypes.SatelliteCount), 0)
        End Get
        Set(value As Integer)
            SetStatistic(StatisticTypes.SatelliteCount, value)
        End Set
    End Property

    Public Property Authority As Integer Implements IGroup.Authority
        Get
            Return GetStatistic(StatisticTypes.Authority).Value
        End Get
        Set(value As Integer)
            SetStatistic(StatisticTypes.Authority, value)
        End Set
    End Property

    Public Property Standards As Integer Implements IGroup.Standards
        Get
            Return GetStatistic(StatisticTypes.Standards).Value
        End Get
        Set(value As Integer)
            SetStatistic(StatisticTypes.Standards, value)
        End Set
    End Property

    Public Property Conviction As Integer Implements IGroup.Conviction
        Get
            Return GetStatistic(StatisticTypes.Conviction).Value
        End Get
        Set(value As Integer)
            SetStatistic(StatisticTypes.Conviction, value)
        End Set
    End Property

    Public ReadOnly Property GroupType As String Implements IGroup.GroupType
        Get
            Return GetMetadata(MetadataTypes.GroupType)
        End Get
    End Property

    Private Property IGroup_Group As IGroup Implements IGroup.LegacyGroup
        Get
            Return Group.FromId(UniverseData, GetStatistic(StatisticTypes.LegacyGroupId))
        End Get
        Set(value As IGroup)
            SetStatistic(StatisticTypes.LegacyGroupId, value?.Id)
        End Set
    End Property

    Public ReadOnly Property Parents As IEnumerable(Of IGroup) Implements IGroup.Parents
        Get
            Return EntityData.Parents.Select(Function(x) Group.FromId(UniverseData, x))
        End Get
    End Property

    Public ReadOnly Property Children As IEnumerable(Of IGroup) Implements IGroup.Children
        Get
            Return EntityData.Children.Select(Function(x) Group.FromId(UniverseData, x))
        End Get
    End Property
End Class
