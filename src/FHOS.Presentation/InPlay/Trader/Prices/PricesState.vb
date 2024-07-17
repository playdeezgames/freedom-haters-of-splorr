Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class PricesState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        If Not model.State.Avatar.Trader.Prices.Any(Function(x) x.MaximumQuantity > 0) Then
            Return New TraderState(model, ui, endState)
        End If
        ui.Clear()
        ui.WriteLine((Mood.Info, $"Jools: {model.State.Avatar.Jools}"))
        Dim menuItems As New List(Of (String, IAvatarTraderPriceModel)) From
            {
                (Choices.Leave, Nothing)
            }
        menuItems.AddRange(model.State.Avatar.Trader.Prices.Select(Function(x) ($"{x.Name}@{x.UnitPrice} (You have {x.InventoryQuantity})", x)))
        Dim choice = ui.Choose((Mood.Prompt, String.Empty), menuItems.ToArray)
        If choice Is Nothing Then
            Return New TraderState(model, ui, endState)
        End If
        Return New BuyQuantityState(model, ui, endState, choice)
    End Function
End Class
