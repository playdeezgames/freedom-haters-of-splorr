Imports FHOS.Data
Imports FHOS.Persistence

Friend MustInherit Class BaseDialog
    Implements IDialog
    Protected ReadOnly Property Actor As IActor
    Sub New(actor As IActor)
        Me.Actor = actor
    End Sub
    Public MustOverride ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String)) Implements IDialog.Lines
    Public MustOverride ReadOnly Property Choices As IEnumerable(Of (Text As String, Value As Action)) Implements IDialog.Choices
End Class
