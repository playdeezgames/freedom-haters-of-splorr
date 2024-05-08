Friend Class Faction
    Inherits FactionDataClient
    Implements IFaction

    Public Sub New(universeData As Data.UniverseData, factionId As Integer)
        MyBase.New(universeData, factionId)
    End Sub

    Public ReadOnly Property MinimumPlanetCount As Integer Implements IFaction.MinimumPlanetCount
        Get
            Return EntityData.Statistics(StatisticTypes.MinimumPlanetCount)
        End Get
    End Property

    Public Function HasFlag(flag As String) As Boolean Implements IFaction.HasFlag
        Return EntityData.Flags.Contains(flag)
    End Function
End Class
