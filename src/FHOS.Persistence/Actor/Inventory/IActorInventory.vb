Public Interface IActorInventory
    Sub Add(item As IItem)
    Sub Remove(item As IItem)
    ReadOnly Property Items As IEnumerable(Of IItem)
    ReadOnly Property ItemCountOfType(itemType As String) As Integer
End Interface
