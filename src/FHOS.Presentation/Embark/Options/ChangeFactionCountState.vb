Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class ChangeFactionCountState
    Inherits EmbarkOptionState(Of Integer)
    Implements IState

    Public Sub New(
                  model As IUniverseModel,
                  ui As IUIContext,
                  endState As IState)
        MyBase.New(
            model,
            ui,
            endState,
            Prompts.FactionCount,
            0)
    End Sub

    Protected Overrides Sub OnOption(answer As Integer)
        model.Settings.FactionCount.SetFactionCount(answer)
    End Sub
    Protected Overrides Function OptionSource() As IEnumerable(Of (Text As String, Item As Integer))
        Return model.Settings.FactionCount.Options
    End Function

    Protected Overrides Function IsCancel(answer As Integer) As Boolean
        Return answer = 0
    End Function
End Class
