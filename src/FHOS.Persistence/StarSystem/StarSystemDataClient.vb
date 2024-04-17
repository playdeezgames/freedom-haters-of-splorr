Imports FHOS.Data

Friend Class StarSystemDataClient
    Inherits UniverseDataClient
    Protected ReadOnly StarSystemId As Integer
    Protected ReadOnly Property StarSystemData As StarSystemData
        Get
            Return UniverseData.LegacyStarSystems(StarSystemId)
        End Get
    End Property
    Sub New(worldData As UniverseData, starSystemId As Integer)
        MyBase.New(worldData)
        Me.StarSystemId = starSystemId
    End Sub
End Class
