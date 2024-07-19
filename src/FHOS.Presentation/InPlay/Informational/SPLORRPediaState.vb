Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class SPLORRPediaState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        Select Case ui.Choose(
            (Mood.Prompt, Prompts.SPLORRPedia),
            Choices.Cancel,
            Choices.Factions,
            Choices.StarSystems,
            Choices.Planets,
            Choices.Satellites)
            Case Choices.Cancel
                Return New ActionMenuState(model, ui, endState)
            Case Else
                Throw New NotImplementedException
        End Select
    End Function
End Class
