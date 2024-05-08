Imports FHOS.Data

Friend Class ActorDataClient
    Inherits EntityDataClient
    Sub New(universeData As UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub
    Protected ReadOnly Property ActorData As ActorData
        Get
            Return UniverseData.Actors.Entities(Id)
        End Get
    End Property
End Class
