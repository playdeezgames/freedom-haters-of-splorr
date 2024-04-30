Friend Class Place
    Inherits PlaceDataClient
    Implements IPlace

    Public Sub New(universeData As Data.UniverseData, starId As Integer)
        MyBase.New(universeData, starId)
    End Sub

    Public ReadOnly Property Id As Integer Implements IPlace.Id
        Get
            Return PlaceId
        End Get
    End Property
End Class
