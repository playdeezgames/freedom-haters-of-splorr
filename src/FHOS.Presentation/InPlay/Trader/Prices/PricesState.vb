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
End Class
