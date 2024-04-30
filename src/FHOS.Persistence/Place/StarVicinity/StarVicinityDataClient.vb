Imports FHOS.Data

Friend Class StarVicinityDataClient
    Inherits UniverseDataClient
    Protected ReadOnly StarVicinityId As Integer
    Protected ReadOnly Property StarVicinityData As StarVicinityData
        Get
            Return UniverseData.StarVicinities.Entities(StarVicinityId)
        End Get
    End Property
    Public Sub New(universeData As Data.UniverseData, starId As Integer)
        MyBase.New(universeData)
        Me.StarVicinityId = starId
    End Sub
End Class
