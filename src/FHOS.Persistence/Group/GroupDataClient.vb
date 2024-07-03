Imports FHOS.Data

Friend Class GroupDataClient
    Inherits NamedEntityDataClient(Of GroupData)
    Public Sub New(
                  universeData As Data.IUniverseData,
                  factionId As Integer)
        MyBase.New(
            universeData,
            factionId,
            Function(u, i) u.Groups(i),
            Sub(u, i) Return)
    End Sub
End Class
