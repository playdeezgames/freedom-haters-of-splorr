Friend Class Star
    Inherits StarDataClient
    Implements IStar

    Public Sub New(universeData As Data.UniverseData, starId As Integer)
        MyBase.New(universeData, starId)
    End Sub

    Public ReadOnly Property Id As Integer Implements IStar.Id
        Get
            Return StarId
        End Get
    End Property
End Class
