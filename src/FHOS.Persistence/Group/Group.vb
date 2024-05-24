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
End Class
