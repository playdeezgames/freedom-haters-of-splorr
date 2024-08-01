Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class PricesState
    Inherits FilteredModelState(Of IAvatarTraderPriceModel)

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState, filter As String)
        MyBase.New(model, ui, endState, filter)
    End Sub

    Protected Overrides ReadOnly Property ModelSource As IEnumerable(Of IAvatarTraderPriceModel)
        Get
            Return model.State.Avatar.Yokes.Trader.Prices
        End Get
    End Property

    Protected Overrides ReadOnly Property PromptText As String
        Get
            Return $"Jools: {model.State.Avatar.Jools}"
        End Get
    End Property

    Protected Overrides Function ApplyFilter(filter As String) As IState
        Return New PricesState(model, ui, endState, filter)
    End Function

    Protected Overrides Function OnSelected(model As IAvatarTraderPriceModel) As IState
        Return New BuyQuantityState(Me.model, ui, endState, model)
    End Function

    Protected Overrides Function ToName(model As IAvatarTraderPriceModel) As String
        Return $"{model.Name}@{model.UnitPrice} (You have {model.InventoryQuantity})"
    End Function

    Protected Overrides Function OnCancel() As IState
        Return New TraderState(model, ui, endState)
    End Function
    'Inherits BaseState
    'Implements IState

    'Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
    '    MyBase.New(model, ui, endState)
    'End Sub

    'Public Overrides Function Run() As IState
    '    ui.Clear()
    '    ui.WriteLine((Mood.Info, $"Jools: {model.State.Avatar.Jools}"))
    '    Dim menuItems As New List(Of (String, IAvatarTraderPriceModel)) From
    '        {
    '            (Choices.Cancel, Nothing)
    '        }
    '    menuItems.AddRange(model.State.Avatar.Trader.Prices.Select(Function(x) ($"{x.Name}@{x.UnitPrice} (You have {x.InventoryQuantity})", x)))
    '    Dim choice = ui.Choose((Mood.Prompt, String.Empty), menuItems.ToArray)
    '    If choice Is Nothing Then
    '        Return New TraderState(model, ui, endState)
    '    End If
    '    Return New BuyQuantityState(model, ui, endState, choice)
    'End Function
End Class
