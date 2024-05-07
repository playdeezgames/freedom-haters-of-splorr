Friend Class Faction
    Inherits FactionDataClient
    Implements IFaction

    Public Sub New(universeData As Data.UniverseData, factionId As Integer)
        MyBase.New(universeData, factionId)
    End Sub
End Class
