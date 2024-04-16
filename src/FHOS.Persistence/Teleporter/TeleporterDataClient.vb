Imports FHOS.Data

Friend Class TeleporterDataClient
    Inherits WorldDataClient
    Protected ReadOnly Property TeleporterId As Integer
    Protected ReadOnly Property TeleporterData As TeleporterData
        Get
            Return WorldData.Teleporters(TeleporterId)
        End Get
    End Property
    Public Sub New(worldData As Data.WorldData, teleporterId As Integer)
        MyBase.New(worldData)
        Me.TeleporterId = teleporterId
    End Sub
End Class
