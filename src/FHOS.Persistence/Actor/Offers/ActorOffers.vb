Imports FHOS.Data

Friend Class ActorOffers
    Inherits ActorDataClient
    Implements IActorOffers

    Protected Sub New(universeData As IUniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Public ReadOnly Property HasAny As Boolean Implements IActorOffers.HasAny
        Get
            Return EntityData.HasOffers
        End Get
    End Property

    Public ReadOnly Property All As IEnumerable(Of String) Implements IActorOffers.All
        Get
            Return EntityData.AllOffers
        End Get
    End Property

    Public Sub Add(itemType As String) Implements IActorOffers.Add
        EntityData.AddOffer(itemType)
    End Sub

    Friend Shared Function FromId(universeData As IUniverseData, id As Integer) As IActorOffers
        Return New ActorOffers(universeData, id)
    End Function
End Class
