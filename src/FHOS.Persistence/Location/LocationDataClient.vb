Imports FHOS.Data

Friend Class LocationDataClient
    Inherits EntityDataClient

    Public Sub New(universeData As UniverseData, locationId As Integer)
        MyBase.New(universeData, locationId)
    End Sub
    Protected ReadOnly Property LocationData As LocationData
        Get
            Return UniverseData.Locations.Entities(Id)
        End Get
    End Property
End Class
