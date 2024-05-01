Imports FHOS.Data

Friend Class StarSystemDataClient
    Inherits UniverseDataClient
    Protected ReadOnly PlaceId As Integer
    Protected ReadOnly Property PlaceData As PlaceData
        Get
            Return UniverseData.Places.Entities(PlaceId)
        End Get
    End Property
    Sub New(worldData As UniverseData, starSystemId As Integer)
        MyBase.New(worldData)
        Me.PlaceId = starSystemId
    End Sub
End Class
