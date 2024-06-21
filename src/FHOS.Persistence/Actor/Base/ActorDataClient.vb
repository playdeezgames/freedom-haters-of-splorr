Imports FHOS.Data
Imports Microsoft.Data.Sqlite

Friend Class ActorDataClient
    Inherits NamedEntityDataClient(Of IActorData)
    Protected Sub New(
                     universeData As IUniverseData,
                     connection As SqliteConnection,
                     actorId As Integer)
        MyBase.New(
            universeData,
            connection,
            actorId,
            Function(u, i) u.GetActorData(i),
            Sub(u, i) Return)
    End Sub
    Public Overrides Sub Recycle()
        Dim actor = Persistence.Actor.FromId(UniverseData, Connection, Id)
        actor.Location.Actor = Nothing
        MyBase.Recycle()
    End Sub
End Class
