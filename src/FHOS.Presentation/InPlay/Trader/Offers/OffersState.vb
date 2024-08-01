Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class OffersState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        If Not model.State.Avatar.Yokes.Trader.Offers.Any(Function(x) x.Quantity > 0) Then
            Return New TraderState(model, ui, endState)
        End If
        ui.Clear()
        ui.WriteLine((Mood.Info, $"Jools: {model.State.Avatar.Jools}"))
        Dim menuItems As New List(Of (String, IAvatarTraderOfferModel)) From
            {
                (Choices.Cancel, Nothing)
            }
        menuItems.AddRange(model.State.Avatar.Yokes.Trader.Offers.Select(Function(x) ($"{x.NameAndQuantity}@{x.JoolsOffered(1)}", x)))
        Dim choice = ui.Choose((Mood.Prompt, String.Empty), menuItems.ToArray)
        If choice Is Nothing Then
            Return New TraderState(model, ui, endState)
        End If
        Return New SellQuantityState(model, ui, endState, choice)
    End Function
End Class
