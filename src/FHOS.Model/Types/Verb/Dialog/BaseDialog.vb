Imports FHOS.Data
Imports FHOS.Persistence

Friend MustInherit Class BaseDialog
    Implements IDialog
    Protected ReadOnly Property Actor As IActor
    Protected ReadOnly finalDialog As IDialog
    Sub New(actor As IActor, finalDialog As IDialog)
        Me.Actor = actor
        Me.finalDialog = finalDialog
    End Sub
    Protected Function EndDialog() As IDialog
        Return finalDialog
    End Function
    Public MustOverride ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String)) Implements IDialog.Lines
    Public MustOverride ReadOnly Property Choices As IEnumerable(Of (Text As String, Value As Func(Of IDialog))) Implements IDialog.Choices
End Class
