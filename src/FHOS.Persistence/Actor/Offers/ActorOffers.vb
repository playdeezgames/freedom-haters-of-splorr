Imports FHOS.Data

Friend Class ActorOffers
    Inherits ActorDataClient
    Implements IActorOffers

    Protected Sub New(universeData As UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Public ReadOnly Property HasAny(seller As IActor) As Boolean Implements IActorOffers.HasAny
        Get
            Return All(seller).Any
        End Get
    End Property

    Public ReadOnly Property All(seller As IActor) As IEnumerable(Of String) Implements IActorOffers.All
        Get
            Return EntityData.AllOffers.Where(Function(x) seller.Inventory.AnyOfType(x))
        End Get
    End Property

    Public ReadOnly Property Actor As IActor Implements IActorOffers.Actor
        Get
            Return Persistence.Actor.FromId(UniverseData, Id)
        End Get
    End Property

    Public Sub Add(itemType As String) Implements IActorOffers.Add
        EntityData.AddOffer(itemType)
    End Sub

    Friend Shared Function FromId(universeData As UniverseData, id As Integer) As IActorOffers
        Return New ActorOffers(universeData, id)
    End Function
End Class
