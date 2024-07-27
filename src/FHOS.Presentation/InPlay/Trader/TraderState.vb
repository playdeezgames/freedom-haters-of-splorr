Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class TraderState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        With model.State.Avatar.Trader
            Dim result As New List(Of String) From
                {
                    Choices.Cancel
                }
            If .HasPrices Then
                result.Add(Choices.Buy)
            End If
            If .HasOffers Then
                result.Add(Choices.Sell)
            End If
            Dim choice = ui.Choose((Mood.Prompt, .Specimen.Name), result.ToArray)
            Select Case choice
                Case Choices.Buy
                    Return New PricesState(model, ui, endState, String.Empty)
                Case Choices.Sell
                    Return New OffersState(model, ui, endState)
                Case Else
                    .Leave()
                    Return New NeutralState(model, ui, endState)
            End Select
        End With
    End Function
End Class
