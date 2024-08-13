Imports FHOS.Data

Friend Class DestinationBlockedDialog
    Inherits BaseMessageMenuDialog

    Public Sub New(actor As Persistence.IActor, finalDialog As IDialog)
        MyBase.New(actor, finalDialog)
    End Sub

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Return {
                    (Hues.Orange, "Destination Blocked!"),
                    (Hues.Red, "The other end is blocked!")
                }
    End Function
End Class
