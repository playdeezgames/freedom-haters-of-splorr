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

    Public ReadOnly Property Choices As IEnumerable(Of String) Implements IAvatarStateDialogModel.Choices
        Get
            Return dialog.Menu
        End Get
    End Property

    Public ReadOnly Property Prompt As String Implements IAvatarStateDialogModel.Prompt
        Get
            Return dialog.Prompt
        End Get
    End Property

    Friend Shared Function FromDialog(actor As IActor, dialog As IDialog) As IAvatarStateDialogModel
        Return If(dialog IsNot Nothing, New AvatarStateDialogModel(actor, dialog), Nothing)
    End Function

    Public Sub Choose(choice As String) Implements IAvatarStateDialogModel.Choose
        actor.Dialog = dialog.Choose(choice)
    End Sub
End Class
