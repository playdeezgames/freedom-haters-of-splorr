Imports FHOS.Data
Imports FHOS.Persistence

Friend MustInherit Class BaseDialog
    Implements IDialog
    Protected ReadOnly Property Actor As IActor
    Protected ReadOnly finalDialog As IDialog
    Sub New(
           dialogType As DialogType,
           actor As IActor,
           finalDialog As IDialog,
           Optional prompt As String = Nothing,
           Optional defaultInput As String = Nothing)
        Me.Actor = actor
        Me.finalDialog = finalDialog
        Me.DialogType = dialogType
        Me.DefaultInput = defaultInput
        Me.Prompt = If(prompt, String.Empty)
    End Sub
    Protected Function EndDialog() As IDialog
        Return finalDialog
    End Function

    Public ReadOnly Property DialogType As DialogType Implements IDialog.DialogType
    Public ReadOnly Property DefaultInput As String Implements IDialog.DefaultInput
    Public ReadOnly Property Prompt As String Implements IDialog.Prompt

    Public MustOverride ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String)) Implements IDialog.Lines
    Public MustOverride ReadOnly Property Menu As IEnumerable(Of String) Implements IDialog.Menu
    Public MustOverride Function Choose(choice As String) As IDialog Implements IDialog.Choose
End Class
