Friend Class Teleporter
    Inherits TeleporterDataClient
    Implements ITeleporter

    Public Sub New(worldData As Data.WorldData, teleporterId As Integer)
        MyBase.New(worldData, teleporterId)
    End Sub

    Public ReadOnly Property Id As Integer Implements ITeleporter.Id
        Get
            Return TeleporterId
        End Get
    End Property
End Class
