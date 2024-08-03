Public Interface IAvatarStateModel
    ReadOnly Property Position As (X As Integer, Y As Integer)
    ReadOnly Property MapName As String
    ReadOnly Property HasDialog As Boolean
    ReadOnly Property Dialog As IAvatarStateDialogModel
End Interface
