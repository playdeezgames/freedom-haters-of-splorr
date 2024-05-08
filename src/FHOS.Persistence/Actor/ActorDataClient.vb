Imports FHOS.Data

Friend Class ActorDataClient
    Inherits EntityDataClient(Of ActorData)
    Sub New(universeData As UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId, Function(u, i) u.Actors.Entities(i))
    End Sub
End Class
