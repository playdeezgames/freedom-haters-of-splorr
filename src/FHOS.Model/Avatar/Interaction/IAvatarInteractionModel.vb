Public Interface IAvatarInteractionModel
    Sub Leave()
    ReadOnly Property IsActive As Boolean
    ReadOnly Property AvailableChoices As List(Of (Text As String, Item As IInteractionModel))
End Interface
