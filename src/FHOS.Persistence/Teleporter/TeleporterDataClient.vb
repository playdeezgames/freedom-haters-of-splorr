Imports FHOS.Data

Friend Class TeleporterDataClient
    Inherits EntityDataClient
    Protected ReadOnly Property TeleporterData As TeleporterData
        Get
            Return UniverseData.Teleporters.Entities(Id)
        End Get
    End Property
    Public Sub New(universeData As Data.UniverseData, teleporterId As Integer)
        MyBase.New(universeData, teleporterId)
    End Sub
End Class
