Imports FHOS.Data

Friend Class TeleporterDataClient
    Inherits EntityDataClient(Of TeleporterData)
    Public Sub New(universeData As Data.UniverseData, teleporterId As Integer)
        MyBase.New(universeData, teleporterId, Function(u, i) u.Teleporters.Entities(i))
    End Sub
End Class
