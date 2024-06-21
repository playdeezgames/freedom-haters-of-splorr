Imports FHOS.Data
Imports Microsoft.Data.Sqlite

Friend Class GroupDataClient
    Inherits NamedEntityDataClient(Of IGroupData)
    Public Sub New(
                  universeData As Data.IUniverseData,
                  connection As SqliteConnection,
                  factionId As Integer)
        MyBase.New(
            universeData,
            connection,
            factionId,
            Function(u, i) u.Groups(i),
            Sub(u, i) Return)
    End Sub
End Class
