Friend Class EntityDataClient
    Inherits UniverseDataClient
    Implements IEntity
    Private ReadOnly entityId As Integer

    Public ReadOnly Property Id As Integer Implements IEntity.Id
        Get
            Return EntityId
        End Get
    End Property

    Public Sub New(universeData As Data.UniverseData, entityId As Integer)
        MyBase.New(universeData)
        Me.EntityId = entityId
    End Sub
End Class
