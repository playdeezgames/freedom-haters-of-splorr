Imports FHOS.Data

Friend Class EmergencyRefuelDialog
    Inherits BaseDialog
    Private result As List(Of (Hue As Integer, Text As String)) = Nothing

    Public Sub New(actor As Persistence.IActor, finalDialog As IDialog)
        MyBase.New(actor, finalDialog)
    End Sub

    Public Overrides ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String))
        Get
            If result Is Nothing Then
                Dim fuelAdded = Actor.FuelTank.MaximumValue.Value - Actor.FuelTank.CurrentValue
                Dim fuelPrice = 1 'TODO: don't just pick a magic number!
                Dim price = fuelPrice * fuelAdded
                Actor.FuelTank.CurrentValue = Actor.FuelTank.MaximumValue.Value
                Actor.Yokes.Store(YokeTypes.Wallet).CurrentValue -= fuelAdded * fuelPrice
                result = New List(Of (Hue As Integer, Text As String)) From
                    {
                        (Hues.Orange, "Emergency Refuel!"),
                        (Hues.LightGray, $"Added {fuelAdded} fuel!"),
                        (Hues.LightGray, $"Price {price} jools!")
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
