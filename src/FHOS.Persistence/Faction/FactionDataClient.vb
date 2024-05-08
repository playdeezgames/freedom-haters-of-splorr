Imports FHOS.Data

Friend Class FactionDataClient
    Inherits EntityDataClient(Of FactionData)
    Public Sub New(universeData As Data.UniverseData, factionId As Integer)
        MyBase.New(universeData, factionId, Function(u, i) u.Factions.Entities(i))
    End Sub
End Class
