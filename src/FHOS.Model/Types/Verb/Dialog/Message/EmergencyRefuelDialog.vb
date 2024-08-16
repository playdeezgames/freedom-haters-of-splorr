Imports FHOS.Data

Friend Class EmergencyRefuelDialog
    Inherits BaseMessageMenuDialog

    Public Sub New(actor As Persistence.IActor, finalDialog As IDialog)
        MyBase.New(actor, finalDialog)
    End Sub

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Dim fuelAdded = Actor.FuelTank.MaximumValue.Value - Actor.FuelTank.CurrentValue
        Dim fuelPrice = 1 'TODO: don't just pick a magic number!
        Dim price = fuelPrice * fuelAdded
        Actor.FuelTank.CurrentValue = Actor.FuelTank.MaximumValue.Value
        actor.ChangeJools(-fuelAdded * fuelPrice)
        Return New List(Of (Hue As Integer, Text As String)) From
                    {
                        (Hues.Orange, "Emergency Refuel!"),
                        (Hues.LightGray, $"Added {fuelAdded} fuel!"),
                        (Hues.LightGray, $"Price {price} jools!")
                    }
    End Function
End Class
