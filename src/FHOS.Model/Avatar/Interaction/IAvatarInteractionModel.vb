Public Interface IAvatarInteractionModel
    Sub Leave()
    ReadOnly Property IsActive As Boolean
    ReadOnly Property AvailableChoices As List(Of (Text As String, Item As IInteractionModel))
    ReadOnly Property Lines As IEnumerable(Of (Text As String, Hue As Integer))
End Interface
