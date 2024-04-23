Imports FHOS.Data

Friend Class StarDataClient
    Inherits UniverseDataClient
    Protected ReadOnly StarId As Integer
    Protected ReadOnly Property StarData As StarVicinity
        Get
            Return UniverseData.StarVicinities.Entities(StarId)
        End Get
    End Property
    Public Sub New(universeData As Data.UniverseData, starId As Integer)
        MyBase.New(universeData)
        Me.StarId = starId
    End Sub
End Class
