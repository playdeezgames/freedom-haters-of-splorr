Imports FHOS.Data

Friend Class PlaceDataClient
    Inherits UniverseDataClient

    Protected ReadOnly PlaceId As Integer
    Protected ReadOnly Property PlaceData As PlaceData
        Get
            Return UniverseData.Places.Entities(PlaceId)
        End Get
    End Property

    Public Sub New(universeData As Data.UniverseData, placeId As Integer)
        MyBase.New(universeData)
        Me.PlaceId = placeId
    End Sub
End Class
