Imports FHOS.Persistence

Friend Class MessageModel
    Implements IMessageModel

    Private ReadOnly message As IMessage

    Protected Sub New(message As IMessage)
        Me.message = message
    End Sub

    Friend Shared Function FromMessage(message As IMessage) As IMessageModel
        If message Is Nothing Then
            Return Nothing
        End If
        Return New MessageModel(message)
    End Function

    Public ReadOnly Property Header As String Implements IMessageModel.Header
        Get
            Return message.Header
        End Get
    End Property

    Public ReadOnly Property Lines As IEnumerable(Of (Text As String, Hue As Integer)) Implements IMessageModel.Lines
        Get
            Return message.Lines
        End Get
    End Property
End Class
