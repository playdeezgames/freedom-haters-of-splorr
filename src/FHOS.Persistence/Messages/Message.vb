Imports FHOS.Data

Friend Class Message
    Implements IMessage

    Private ReadOnly messageData As MessageData

    Public Sub New(messageData As MessageData)
        Me.messageData = messageData
    End Sub

    Public ReadOnly Property Header As String Implements IMessage.Header
        Get
            Return messageData.Header
        End Get
    End Property

    Public ReadOnly Property Lines As IEnumerable(Of (Text As String, Hue As Integer)) Implements IMessage.Lines
        Get
            Return messageData.Lines.Select(Function(x) (x.Text, x.Hue))
        End Get
    End Property
End Class
