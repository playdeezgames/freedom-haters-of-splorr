Imports FHOS.Data

Friend Class RefueledDialog
    Inherits BaseMessageMenuDialog

    Public Sub New(actor As Persistence.IActor, finalDialog As IDialog)
        MyBase.New(actor, finalDialog)
    End Sub

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Dim fuelRequired = Actor.FuelTank.TopOffAmount.Value
        Const fuelPerJools = 3
        Dim fuelCost = (fuelRequired + fuelPerJools - 1) \ fuelPerJools
        actor.ChangeJools(-fuelCost)
        actor.FuelTank.CurrentValue += fuelRequired
        Return New List(Of (Hue As Integer, Text As String)) From
                    {
                        (Hues.Orange, "Refueled!"),
                        (Hues.LightGray, $"You bought {fuelRequired} fuel."),
                        (Hues.LightGray, $"You paid {fuelCost} Jools.")
                    }
    End Function
End Class
