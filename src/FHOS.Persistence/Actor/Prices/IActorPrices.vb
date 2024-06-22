Public Interface IActorPrices
    ReadOnly Property HasAny As Boolean
    ReadOnly Property All As IEnumerable(Of String)
    Sub Add(itemType As String)
End Interface
