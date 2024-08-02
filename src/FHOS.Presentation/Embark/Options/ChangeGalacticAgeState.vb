Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class ChangeGalacticAgeState
    Inherits EmbarkOptionState(Of String)
    Implements IState

    Public Sub New(
                  model As IUniverseModel,
                  ui As IUIContext,
                  endState As IState)
        MyBase.New(
            model,
            ui,
            endState,
            Prompts.GalacticAge,
            Choices.Cancel)
    End Sub

    Protected Overrides Sub OnOption(answer As String)
        model.Settings.GalacticAge.SetAge(answer)
    End Sub

    Protected Overrides Function OptionSource() As IEnumerable(Of (Text As String, Item As String))
        Return model.Settings.GalacticAge.Options
    End Function

    Protected Overrides Function IsCancel(answer As String) As Boolean
        Return answer = Choices.Cancel
    End Function
End Class
