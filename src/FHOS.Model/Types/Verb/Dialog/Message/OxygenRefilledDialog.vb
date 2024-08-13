Imports FHOS.Data

Friend Class OxygenRefilledDialog
    Inherits BaseMenuDialog

    Public Sub New(actor As Persistence.IActor, finalDialog As IDialog)
        MyBase.New(actor, finalDialog)
    End Sub

    Protected Overrides Function InitializeMenu() As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Return New Dictionary(Of String, Func(Of IDialog)) From {{DialogChoices.Ok, AddressOf EndDialog}}
    End Function

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Dim oxygenRequired = Actor.LifeSupport.TopOffAmount.Value
        Const oxygenPerJools = 10
        Dim oxygenCost = (oxygenRequired + oxygenPerJools - 1) \ oxygenPerJools
        Actor.Yokes.Store(YokeTypes.Wallet).CurrentValue -= oxygenCost
        Actor.LifeSupport.CurrentValue += oxygenRequired
        Return New List(Of (Hue As Integer, Text As String)) From
                    {
                        (Hues.Orange, "Oxygen Refilled!"),
                        (Hues.LightGray, $"You buy {oxygenRequired} oxygen!"),
                        (Hues.LightGray, $"Cost: {oxygenCost} Jools!")
                    }
    End Function
End Class
