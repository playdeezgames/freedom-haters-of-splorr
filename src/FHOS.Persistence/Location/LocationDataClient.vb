Imports FHOS.Data

Friend Class LocationDataClient
    Inherits UniverseDataClient

    Public Sub New(universeData As UniverseData, locationId As Integer)
        MyBase.New(universeData)
        Me.LocationId = locationId
    End Sub

    Protected ReadOnly Property LocationId As Integer
    Protected ReadOnly Property LocationData As LocationData
        Get
            Return UniverseData.Locations.Entities(LocationId)
        End Get
    End Property
End Class
