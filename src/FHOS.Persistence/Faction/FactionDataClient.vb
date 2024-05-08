Imports FHOS.Data

Friend Class FactionDataClient
    Inherits EntityDataClient
    Protected ReadOnly Property FactionData As FactionData
        Get
            Return UniverseData.Factions.Entities(Id)
        End Get
    End Property
    Public Sub New(universeData As Data.UniverseData, factionId As Integer)
        MyBase.New(universeData, factionId)
    End Sub
End Class
