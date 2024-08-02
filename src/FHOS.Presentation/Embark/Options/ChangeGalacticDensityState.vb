Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class ChangeGalacticDensityState
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
            Prompts.GalacticDensity,
            Choices.Cancel)
    End Sub

    Protected Overrides Sub OnOption(answer As String)
        model.Settings.GalacticDensity.SetDensity(answer)
    End Sub

    Protected Overrides Function OptionSource() As IEnumerable(Of (Text As String, Item As String))
        Return model.Settings.GalacticDensity.Options
    End Function

    Protected Overrides Function IsCancel(answer As String) As Boolean
        Return answer = Choices.Cancel
    End Function
End Class
