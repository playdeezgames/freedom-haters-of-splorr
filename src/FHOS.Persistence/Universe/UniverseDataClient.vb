Imports FHOS.Data

Public Class UniverseDataClient
    Protected ReadOnly UniverseData As IUniverseData
    Sub New(universeData As IUniverseData)
        Me.UniverseData = universeData
    End Sub
End Class
