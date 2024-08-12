Imports FHOS.Data
Imports FHOS.Persistence

Friend Class AvatarStateDialogModel
    Implements IAvatarStateDialogModel

    Private ReadOnly dialog As IDialog
    Public ReadOnly Property actor As IActor

    Public Sub New(actor As IActor, dialog As IDialog)
        Me.dialog = dialog
        Me.actor = actor
    End Sub

    Public ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String)) Implements IAvatarStateDialogModel.Lines
        Get
            Return dialog.Lines
        End Get
    End Property

    Public ReadOnly Property Choices As IEnumerable(Of (Text As String, Value As Action)) Implements IAvatarStateDialogModel.Choices
        Get
            Return dialog.LegacyChoices.Select(Function(x) (x.Text, MakeChoice(x.Value)))
        End Get
    End Property

    Private Function MakeChoice(consequence As Func(Of IDialog)) As Action
        Return Sub()
                   actor.Dialog = consequence()
               End Sub
    End Function

    Friend Shared Function FromDialog(actor As IActor, dialog As IDialog) As IAvatarStateDialogModel
        Return If(dialog IsNot Nothing, New AvatarStateDialogModel(actor, dialog), Nothing)
    End Function
End Class
