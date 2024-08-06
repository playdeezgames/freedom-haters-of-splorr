Imports FHOS.Data

Friend MustInherit Class BaseDialog
    Implements IDialog
    Public MustOverride ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String)) Implements IDialog.Lines
    Public MustOverride ReadOnly Property Choices As IEnumerable(Of (Text As String, Value As Action)) Implements IDialog.Choices
End Class
