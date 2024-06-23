Public Interface IActorInventory
    Sub Add(item As IItem)
    Sub Remove(item As IItem)
    ReadOnly Property Items As IEnumerable(Of IItem)
    ReadOnly Property ItemCountOfType(itemType As String) As Integer
    ReadOnly Property ItemsOfType(itemType As String) As IEnumerable(Of IItem)
End Interface
