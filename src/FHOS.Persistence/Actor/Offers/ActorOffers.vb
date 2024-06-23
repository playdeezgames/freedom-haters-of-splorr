Imports FHOS.Data

Friend Class ActorOffers
    Inherits ActorDataClient
    Implements IActorOffers

    Protected Sub New(universeData As IUniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Public ReadOnly Property LegacyHasAny As Boolean Implements IActorOffers.LegacyHasAny
        Get
            Return EntityData.HasOffers
        End Get
    End Property

    Public ReadOnly Property LegacyAll As IEnumerable(Of String) Implements IActorOffers.LegacyAll
        Get
            Return EntityData.AllOffers
        End Get
    End Property

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

    Friend Shared Function FromId(universeData As IUniverseData, id As Integer) As IActorOffers
        Return New ActorOffers(universeData, id)
    End Function
End Class
