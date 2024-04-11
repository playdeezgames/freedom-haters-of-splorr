Imports FHOS.Data

Friend Class StarSystemDataClient
    Inherits WorldDataClient
    Protected ReadOnly StarSystemId As Integer
    Protected ReadOnly Property StarSystemData As StarSystemData
        Get
            Return WorldData.StarSystems(StarSystemId)
        End Get
    End Property
    Sub New(worldData As WorldData, starSystemId As Integer)
        MyBase.New(worldData)
        Me.StarSystemId = starSystemId
    End Sub
End Class
