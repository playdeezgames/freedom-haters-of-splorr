Public Interface IAvatar
    Sub SetActor(actor As IActor)
    Function RemoveActor() As IActor
    ReadOnly Property Actor As IActor
End Interface
