Friend Class Faction
    Inherits FactionDataClient
    Implements IFaction

    Public Sub New(universeData As Data.UniverseData, factionId As Integer)
        MyBase.New(universeData, factionId)
    End Sub

    Public ReadOnly Property Id As Integer Implements IFaction.Id
        Get
            Return FactionId
        End Get
    End Property

    Public ReadOnly Property MinimumPlanetCount As Integer Implements IFaction.MinimumPlanetCount
        Get
            Return FactionData.Statistics(StatisticTypes.MinimumPlanetCount)
        End Get
    End Property
End Class
