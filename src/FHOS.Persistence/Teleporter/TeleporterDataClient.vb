Imports FHOS.Data

Friend Class TeleporterDataClient
    Inherits UniverseDataClient
    Protected ReadOnly Property TeleporterId As Integer
    Protected ReadOnly Property TeleporterData As TeleporterData
        Get
            Return UniverseData.Teleporters.Entities(TeleporterId)
        End Get
    End Property
    Public Sub New(worldData As Data.UniverseData, teleporterId As Integer)
        MyBase.New(worldData)
        Me.TeleporterId = teleporterId
    End Sub
End Class
