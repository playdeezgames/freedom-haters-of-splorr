Public Interface IAvatar
    Sub Push(actor As IActor)
    Function Pop() As IActor
    ReadOnly Property Actor As IActor
End Interface
