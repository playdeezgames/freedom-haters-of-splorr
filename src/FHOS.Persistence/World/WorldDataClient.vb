Imports FHOS.Data

Public Class WorldDataClient
    Protected ReadOnly WorldData As UniverseData
    Sub New(worldData As UniverseData)
        Me.WorldData = worldData
    End Sub
End Class
