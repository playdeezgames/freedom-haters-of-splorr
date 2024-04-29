Imports FHOS.Model
Imports SPLORR.UI

Friend Class NeutralState
    Inherits BaseGameState(Of IUniverseModel)

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of IUniverseModel))
        MyBase.New(parent, setState, context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub OnStart()
        MyBase.OnStart()
        If Not Context.Model.DoneGenerating Then
            SetState(GameState.Generate)
        ElseIf Context.Model.Messages.HasAny Then
            SetState(GameState.Message)
        ElseIf Context.Model.Avatar.IsGameOver Then
            SetState(GameState.GameOver)
        ElseIf Context.Model.Avatar.Tutorial.HasAny Then
            SetState(GameState.Tutorial)
        Else
            SetState(GameState.Navigation)
        End If

    End Sub
End Class
