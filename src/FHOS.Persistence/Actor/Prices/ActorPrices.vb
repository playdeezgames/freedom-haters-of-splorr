Friend Class ActorPrices
    Inherits ActorDataClient
    Implements IActorPrices

    Protected Sub New(universeData As Data.UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Public ReadOnly Property HasAny As Boolean Implements IActorPrices.HasAny
        Get
            Return EntityData.HasPricedItems
        End Get
    End Property

    Public ReadOnly Property All As IEnumerable(Of String) Implements IActorPrices.All
        Get
            Return EntityData.AllPricedItems
        End Get
    End Property

    Public Sub Add(itemType As String) Implements IActorPrices.Add
        EntityData.AddPricedItem(itemType)
    End Sub

    Friend Shared Function FromId(universeData As Data.UniverseData, id As Integer) As IActorPrices
        Return New ActorPrices(universeData, id)
    End Function
End Class
