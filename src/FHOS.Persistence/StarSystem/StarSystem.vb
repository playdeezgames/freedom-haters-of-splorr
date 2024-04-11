Friend Class StarSystem
    Inherits StarSystemDataClient
    Implements IStarSystem

    Public Sub New(worldData As Data.WorldData, starSystemId As Integer)
        MyBase.New(worldData, starSystemId)
    End Sub

    Public ReadOnly Property Id As Integer Implements IStarSystem.Id
        Get
            Return StarSystemId
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IStarSystem.Name
        Get
            Return StarSystemData.Metadatas(MetadataTypes.Name)
        End Get
    End Property
End Class
