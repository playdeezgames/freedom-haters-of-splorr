Imports FHOS.Data

Friend Class ActorDataClient
    Inherits UniverseDataClient
    Sub New(universeData As UniverseData, actorId As Integer)
        MyBase.New(universeData)
        Me.ActorId = actorId
    End Sub
    Protected ReadOnly Property ActorId As Integer
    Protected ReadOnly Property ActorData As ActorData
        Get
            Return UniverseData.Actors.Entities(ActorId)
        End Get
    End Property
End Class
