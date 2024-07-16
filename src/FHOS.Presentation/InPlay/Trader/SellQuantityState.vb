Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class SellQuantityState
    Inherits BaseState
    Implements IState
    Const QuantityOne = "One"
    Const QuantityAll = "All"
    Const QuantityHalf = "Half"
    Const QuantitySpecificNumber = "Specific Number"

    Private ReadOnly offer As IAvatarTraderOfferModel

    Public Sub New(
                  model As IUniverseModel,
                  ui As IUIContext,
                  endState As IState,
                  offer As IAvatarTraderOfferModel)
        MyBase.New(model, ui, endState)
        Me.offer = offer
    End Sub

    Public Overrides Function Run() As IState
        With offer
            ui.Clear()
            Dim result As New List(Of (Text As String, Item As String)) From
            {
                (Choices.Leave, Choices.Leave),
                ($"{QuantityOne}(1)", QuantityOne),
                ($"{QuantityAll}({ .Quantity})", QuantityAll)
            }
            If .Quantity > 3 Then
                result.Add(($"{QuantityHalf}({ .Quantity \ 2})", QuantityHalf))
            End If
            If .Quantity > 2 Then
                result.Add(($"{QuantitySpecificNumber}...", QuantitySpecificNumber))
            End If
            Select Case ui.Choose((Mood.Prompt, $"Sell How Many { .NameAndQuantity}?"), result.ToArray)
                Case Choices.Leave
                    Return New OffersState(model, ui, endState)
                Case QuantityAll
                    Return New SellConfirmState(model, ui, endState, offer, offer.Quantity)
                Case QuantityOne
                    Return New SellConfirmState(model, ui, endState, offer, 1)
                Case QuantityHalf
                    Return New SellConfirmState(model, ui, endState, offer, offer.Quantity \ 2)
                Case Else
                    Dim quantity = ui.Ask((Mood.Prompt, "How Many?"), 0)
                    If quantity > 0 Then
                        Return New SellConfirmState(model, ui, endState, offer, quantity)
                    End If
                    Return Me
            End Select
        End With
    End Function
End Class
