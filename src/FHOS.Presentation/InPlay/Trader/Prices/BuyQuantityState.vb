Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class BuyQuantityState
    Inherits BaseState
    Implements IState
    Const QuantityOne = "One"
    Const QuantityMaximum = "Maximum"
    Const QuantitySpecificNumber = "Specific Number"

    Private ReadOnly price As IAvatarTraderPriceModel

    Public Sub New(
                  model As IUniverseModel,
                  ui As IUIContext,
                  endState As IState,
                  price As IAvatarTraderPriceModel)
        MyBase.New(model, ui, endState)
        Me.price = price
    End Sub

    Public Overrides Function Run() As IState
        With price
            ui.Clear()
            Dim result As New List(Of (Text As String, Item As String)) From
            {
                (Choices.Leave, Choices.Leave)
            }
            If .MaximumQuantity > 0 Then
                result.Add(($"{QuantityOne}(1)", QuantityOne))
            End If
            If .MaximumQuantity > 1 Then
                result.Add(($"{QuantityMaximum}({ .MaximumQuantity})", QuantityMaximum))
            End If
            If .MaximumQuantity > 2 Then
                result.Add(($"{QuantitySpecificNumber}...", QuantitySpecificNumber))
            End If
            Select Case ui.Choose((Mood.Prompt, $"Buy How Many { .Name}(@{ .UnitPrice})?"), result.ToArray)
                Case Choices.Leave
                    Return New OffersState(model, ui, endState)
                Case QuantityMaximum
                    Return New BuyConfirmState(model, ui, endState, price, price.MaximumQuantity)
                Case QuantityOne
                    Return New BuyConfirmState(model, ui, endState, price, 1)
                Case Else
                    Dim quantity = Math.Clamp(ui.Ask((Mood.Prompt, "How Many?"), 0), 0, price.MaximumQuantity)
                    If quantity > 0 Then
                        Return New BuyConfirmState(model, ui, endState, price, quantity)
                    End If
                    Return Me
            End Select
        End With
    End Function
End Class
