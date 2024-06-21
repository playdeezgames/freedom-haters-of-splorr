Imports FHOS.Data
Imports Microsoft.Data.Sqlite

Friend Class LocationDataClient
    Inherits TypedEntityDataClient(Of ILocationData)

    Public Sub New(
                  universeData As IUniverseData,
                  connection As SqliteConnection,
                  locationId As Integer)
        MyBase.New(
            universeData,
            connection,
            locationId,
            Function(u, i) u.Locations(i),
            Sub(u, i) Return)
    End Sub
End Class
