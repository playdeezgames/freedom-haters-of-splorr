Imports FHOS.Data

Friend Class PlaceDataClient
    Inherits EntityDataClient

    Protected ReadOnly Property PlaceData As PlaceData
        Get
            Return UniverseData.Places.Entities(Id)
        End Get
    End Property

    Public Sub New(universeData As Data.UniverseData, placeId As Integer)
        MyBase.New(universeData, placeId)
    End Sub
End Class
