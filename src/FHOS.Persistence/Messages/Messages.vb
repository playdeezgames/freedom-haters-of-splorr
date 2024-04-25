Imports FHOS.Data

Friend Class Messages
    Implements IMessages

    Private ReadOnly messages As Queue(Of MessageData)

    Public Sub New(messages As Queue(Of MessageData))
        Me.messages = messages
    End Sub

    Public ReadOnly Property HasAny As Boolean Implements IMessages.HasAny
        Get
            Return messages.Any
        End Get
    End Property

    Public ReadOnly Property Current As IMessage Implements IMessages.Current
        Get
            Return New Message(messages.Peek)
        End Get
    End Property

    Public Sub Dismiss() Implements IMessages.Dismiss
        messages.Dequeue()
    End Sub
End Class
