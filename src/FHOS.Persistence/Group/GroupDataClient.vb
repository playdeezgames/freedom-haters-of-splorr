Imports FHOS.Data

Friend Class GroupDataClient
    Inherits TypedEntityDataClient(Of GroupData)
    Public Sub New(
                  universeData As Data.UniverseData,
                  factionId As Integer)
        MyBase.New(
            universeData,
            factionId,
            Function(u, i) u.Groups.Entities(i),
            Sub(u, i) u.Groups.Recycled.Add(i))
    End Sub
End Class
