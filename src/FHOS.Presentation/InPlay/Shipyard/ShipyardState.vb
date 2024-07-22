Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class ShipyardState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        Dim menu As New List(Of (String, String)) From
            {
                (Choices.Cancel, Choices.Cancel)
            }
        Dim choice = ui.Choose(
            (Mood.Prompt, model.State.Avatar.Shipyard.Specimen.Name),
            menu.ToArray)
        Select Case choice
            Case Else
                model.State.Avatar.Shipyard.Leave()
                Return New NeutralState(model, ui, endState)
        End Select
    End Function
End Class
