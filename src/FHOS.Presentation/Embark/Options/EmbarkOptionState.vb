Imports FHOS.Model
Imports SPLORR.Presentation

Friend MustInherit Class EmbarkOptionState(Of TOption)
    Inherits BaseState

    Private ReadOnly prompt As String
    Private ReadOnly cancelValue As TOption

    Public Sub New(
                  model As IUniverseModel,
                  ui As IUIContext,
                  endState As IState,
                  prompt As String,
                  cancelValue As TOption)
        MyBase.New(model, ui, endState)
        Me.prompt = prompt
        Me.cancelValue = cancelValue
    End Sub

    Protected MustOverride Function OptionSource() As IEnumerable(Of (Text As String, Item As TOption))
    Protected MustOverride Sub OnOption(answer As TOption)
    Protected MustOverride Function IsCancel(answer As TOption) As Boolean

    Public Overrides Function Run() As IState
        Dim menu As New List(Of (Text As String, Item As TOption)) From
            {
                (Choices.Cancel, cancelValue)
            }
        menu.AddRange(OptionSource())
        Dim answer = ui.Choose(
            (Mood.Prompt, prompt), menu.ToArray)
        If Not IsCancel(answer) Then
            OnOption(answer)
        End If
        Return endState
    End Function
End Class
