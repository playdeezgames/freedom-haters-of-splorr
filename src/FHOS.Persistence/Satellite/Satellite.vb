Friend Class Satellite
    Inherits PlaceDataClient
    Implements IPlace

    Public Sub New(universeData As Data.UniverseData, satelliteId As Integer)
        MyBase.New(universeData, satelliteId)
    End Sub

    Public ReadOnly Property Id As Integer Implements IPlace.Id
        Get
            Return PlaceId
        End Get
    End Property
End Class
