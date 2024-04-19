Friend Class ConfirmAbandonState(Of TModel)
    Inherits BaseConfirmState(Of TModel)
    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of TModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Are you sure you want to abandon the game?",
            BoilerplateState.MainMenu,
            BoilerplateState.GameMenu)
    End Sub
    Protected Overrides Sub OnConfirm()
        Context.AbandonGame()
    End Sub
End Class
