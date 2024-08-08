Imports FHOS.Data

Friend Class DestinationBlockedDialog
    Inherits BaseDialog

    Public Sub New(actor As Persistence.IActor, finalDialog As IDialog)
        MyBase.New(actor, finalDialog)
    End Sub

    Public Overrides ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String))
        Get
            Return {
                    (Hues.Orange, "Destination Blocked!"),
                    (Hues.Red, "The other end is blocked!")
                }
        End Get
    End Property

    Public Overrides ReadOnly Property Choices As IEnumerable(Of (Text As String, Value As Action))
        Get
            Return {
                (DialogChoices.Ok, AddressOf EndDialog)
                }
        End Get
    End Property
End Class
