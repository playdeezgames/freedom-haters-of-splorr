Friend Class ActorPrices
    Inherits ActorDataClient
    Implements IActorPrices

    Protected Sub New(universeData As Data.UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Public ReadOnly Property HasAny As Boolean Implements IActorPrices.HasAny
        Get
            Return False
        End Get
    End Property

    Friend Shared Function FromId(universeData As Data.UniverseData, id As Integer) As IActorPrices
        Return New ActorPrices(universeData, id)
    End Function
End Class
