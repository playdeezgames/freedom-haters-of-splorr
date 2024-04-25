Public Interface IMessagesModel
    ReadOnly Property HasAny As Boolean
    Sub Dismiss()
    ReadOnly Property Current As IMessageModel
End Interface
