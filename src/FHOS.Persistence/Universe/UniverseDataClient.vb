Imports FHOS.Data
Imports Microsoft.Data.Sqlite

Public Class UniverseDataClient
    Protected ReadOnly UniverseData As IUniverseData
    Protected ReadOnly Connection As SqliteConnection
    Sub New(universeData As IUniverseData, connection As SqliteConnection)
        Me.UniverseData = universeData
        Me.Connection = connection
    End Sub
End Class
