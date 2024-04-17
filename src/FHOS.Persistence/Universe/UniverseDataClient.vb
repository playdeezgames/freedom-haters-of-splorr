Imports FHOS.Data

Public Class UniverseDataClient
    Protected ReadOnly UniverseData As UniverseData
    Sub New(universeData As UniverseData)
        Me.UniverseData = universeData
    End Sub
End Class
