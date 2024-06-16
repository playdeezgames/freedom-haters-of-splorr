Imports FHOS.Data

Friend Class ActorOffers
    Inherits ActorDataClient
    Implements IActorOffers

    Protected Sub New(universeData As UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Public ReadOnly Property HasAny As Boolean Implements IActorOffers.HasAny
        Get
            Return False
        End Get
    End Property

    Friend Shared Function FromId(universeData As UniverseData, id As Integer) As IActorOffers
        Return New ActorOffers(universeData, id)
    End Function
End Class
