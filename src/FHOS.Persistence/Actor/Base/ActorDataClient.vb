Imports FHOS.Data

Friend Class ActorDataClient
    Inherits NamedEntityDataClient(Of ActorData)
    Protected Sub New(
                     universeData As UniverseData,
                     actorId As Integer)
        MyBase.New(
            universeData,
            actorId,
            Function(u, i) u.Actors.Entities(i),
            Sub(u, i) u.Actors.Recycled.Add(i))
    End Sub
    Public Overrides Sub Recycle()
        Dim actor = Persistence.Actor.FromId(UniverseData, Id)
        actor.Location.Actor = Nothing
        MyBase.Recycle()
    End Sub
End Class
