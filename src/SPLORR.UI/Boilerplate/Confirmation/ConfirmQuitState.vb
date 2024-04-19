Friend Class ConfirmQuitState(Of TModel)
    Inherits BaseConfirmState(Of TModel)
    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of TModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Are you sure you want to quit the game?",
            Nothing,
            BoilerplateState.MainMenu)
    End Sub

    Protected Overrides Sub OnConfirm()
        'DO NOTHING!
    End Sub
End Class
