Imports FHOS.Data

Friend Class DestinationBlockedDialog
    Inherits BaseMenuDialog

    Public Sub New(actor As Persistence.IActor, finalDialog As IDialog)
        MyBase.New(actor, finalDialog)
    End Sub

    Protected Overrides Function InitializeMenu() As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Return New Dictionary(Of String, Func(Of IDialog)) From {
                {DialogChoices.Ok, AddressOf EndDialog}
                }
    End Function

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Return {
                    (Hues.Orange, "Destination Blocked!"),
                    (Hues.Red, "The other end is blocked!")
                }
    End Function
End Class
