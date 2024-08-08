Imports FHOS.Data

Friend Class OxygenRefilledDialog
    Inherits BaseDialog
    Private result As List(Of (Hue As Integer, Text As String)) = Nothing

    Public Sub New(actor As Persistence.IActor, finalDialog As IDialog)
        MyBase.New(actor, finalDialog)
    End Sub

    Public Overrides ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String))
        Get
            If result Is Nothing Then
                Dim oxygenRequired = Actor.LifeSupport.TopOffAmount.Value
                Const oxygenPerJools = 10
                Dim oxygenCost = (oxygenRequired + oxygenPerJools - 1) \ oxygenPerJools
                Actor.Yokes.Store(YokeTypes.Wallet).CurrentValue -= oxygenCost
                Actor.LifeSupport.CurrentValue += oxygenRequired
                result = New List(Of (Hue As Integer, Text As String)) From
                    {
                        (Hues.Orange, "Oxygen Refilled!"),
                        (Hues.LightGray, $"You buy {oxygenRequired} oxygen!"),
                        (Hues.LightGray, $"Cost: {oxygenCost} Jools!")
                    }
            End If
            Return result
        End Get
    End Property

    Public Overrides ReadOnly Property Choices As IEnumerable(Of (Text As String, Value As Action))
        Get
            Return {(DialogChoices.Ok, AddressOf EndDialog)}
        End Get
    End Property
End Class
