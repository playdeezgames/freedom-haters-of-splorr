Imports FHOS.Data

Friend Class FactionDataClient
    Inherits UniverseDataClient
    Protected ReadOnly FactionId As Integer
    Protected ReadOnly Property FactionData As FactionData
        Get
            Return UniverseData.Factions.Entities(FactionId)
        End Get
    End Property
    Public Sub New(universeData As Data.UniverseData, factionId As Integer)
        MyBase.New(universeData)
        Me.FactionId = factionId
    End Sub
End Class
