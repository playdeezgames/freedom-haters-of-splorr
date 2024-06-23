Public Interface IActorOffers
    ReadOnly Property HasAny(seller As IActor) As Boolean
    ReadOnly Property All(seller As IActor) As IEnumerable(Of String)
    Sub Add(itemType As String)
    ReadOnly Property Actor As IActor
End Interface
