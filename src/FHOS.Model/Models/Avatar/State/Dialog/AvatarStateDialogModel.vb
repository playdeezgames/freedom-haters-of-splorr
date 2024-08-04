Imports FHOS.Data

Friend Class AvatarStateDialogModel
    Implements IAvatarStateDialogModel

    Private ReadOnly dialog As IDialog

    Public Sub New(dialog As IDialog)
        Me.dialog = dialog
    End Sub

    Public ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String)) Implements IAvatarStateDialogModel.Lines
        Get
            Return dialog.Lines
        End Get
    End Property

    Public ReadOnly Property Choices As IEnumerable(Of (Text As String, Value As Action)) Implements IAvatarStateDialogModel.Choices
        Get
            Return dialog.Choices
        End Get
    End Property

    Friend Shared Function FromDialog(dialog As IDialog) As IAvatarStateDialogModel
        Return If(dialog IsNot Nothing, New AvatarStateDialogModel(dialog), Nothing)
    End Function
End Class
