Imports FHOS.Data

Friend Class ActorDataClient
    Inherits NamedEntityDataClient(Of IActorData)
    Protected Sub New(
                     universeData As IUniverseData,
                     actorId As Integer)
        MyBase.New(
            universeData,
            actorId,
            Function(u, i) u.LegacyActors(i),
            Sub(u, i) Return)
    End Sub
    Public Overrides Sub Recycle()
        Dim actor = Persistence.Actor.FromId(UniverseData, Id)
        actor.Location.Actor = Nothing
        MyBase.Recycle()
    End Sub
End Class
