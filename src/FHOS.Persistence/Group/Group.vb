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

    Public ReadOnly Property MinimumPlanetCount As Integer Implements IGroup.MinimumPlanetCount
        Get
            Return EntityData.Statistics(StatisticTypes.MinimumPlanetCount)
        End Get
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

    Public ReadOnly Property Authority As Integer Implements IGroup.Authority
        Get
            Return GetStatistic(StatisticTypes.Authority).Value
        End Get
    End Property

    Public ReadOnly Property Standards As Integer Implements IGroup.Standards
        Get
            Return GetStatistic(StatisticTypes.Standards).Value
        End Get
    End Property

    Public ReadOnly Property Conviction As Integer Implements IGroup.Conviction
        Get
            Return GetStatistic(StatisticTypes.Conviction).Value
        End Get
    End Property
End Class
