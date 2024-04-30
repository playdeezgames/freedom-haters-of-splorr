Imports FHOS.Data

Friend Class SatelliteDataClient
    Inherits UniverseDataClient

    Public Sub New(universeData As Data.UniverseData, placeId As Integer)
        MyBase.New(universeData)
        Me.PlaceId = placeId
    End Sub

    Protected ReadOnly Property PlaceId As Integer
    Protected ReadOnly Property SatelliteData As PlaceData
        Get
            Return UniverseData.Places.Entities(PlaceId)
        End Get
    End Property
End Class
