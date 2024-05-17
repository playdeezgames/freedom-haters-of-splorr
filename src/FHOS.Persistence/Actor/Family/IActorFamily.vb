Public Interface IActorFamily
    Sub AddChild(child As IActor)
    ReadOnly Property HasChildren As Boolean
    ReadOnly Property Children As IEnumerable(Of IActor)
    Property Parent As IActor
End Interface
