Imports FHOS.Data
Imports FHOS.Persistence

Friend Class AvatarItemDialogModel
    Implements IAvatarItemDialogModel

    Private ReadOnly actor As IActor
    Private ReadOnly dialog As IDialog

    Public Sub New(
                  actor As IActor,
                  dialog As IDialog)
        Me.actor = actor
        Me.dialog = dialog
    End Sub

    Public Sub Start() Implements IAvatarItemDialogModel.Start
        actor.Dialog = dialog
    End Sub

    Friend Shared Function FromDialog(actor As IActor, dialog As IDialog) As IAvatarItemDialogModel
        Return New AvatarItemDialogModel(actor, dialog)
    End Function
End Class
