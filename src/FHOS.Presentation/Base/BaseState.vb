Imports FHOS.Model
Imports SPLORR.Presentation

Public MustInherit Class BaseState
    Implements IState
    Protected ReadOnly model As IUniverseModel
    Protected ReadOnly ui As IUIContext
    Protected ReadOnly endState As IState
    Protected Shared ReadOnly moodTable As IReadOnlyDictionary(Of Integer, Mood) =
        New Dictionary(Of Integer, Mood) From
        {
            {Hues.Black, Mood.Black},
            {Hues.Blue, Mood.Blue},
            {Hues.Green, Mood.Green},
            {Hues.Cyan, Mood.Cyan},
            {Hues.Red, Mood.Red},
            {Hues.Purple, Mood.Purple},
            {Hues.Brown, Mood.Brown},
            {Hues.LightGray, Mood.LightGray},
            {Hues.DarkGray, Mood.DarkGray},
            {Hues.LightBlue, Mood.LightBlue},
            {Hues.LightGreen, Mood.LightGreen},
            {Hues.Tan, Mood.Tan},
            {Hues.Orange, Mood.Orange},
            {Hues.Pink, Mood.Pink},
            {Hues.Yellow, Mood.Yellow},
            {Hues.White, Mood.White}
        }

    Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        Me.model = model
        Me.ui = ui
        Me.endState = endState
    End Sub

    Public MustOverride Function Run() As IState Implements IState.Run
End Class
