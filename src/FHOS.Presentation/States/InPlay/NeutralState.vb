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
        With Context.Model
            If Not .DoneGenerating Then
                SetState(GameState.Generate)
            ElseIf .Messages.HasAny Then
                SetState(GameState.Message)
            ElseIf .Avatar.Status.GameOver Then
                SetState(GameState.GameOver)
            ElseIf .Avatar.Tutorial.HasAny Then
                SetState(GameState.Tutorial)
            ElseIf .Avatar.IsInteracting Then
                SetState(GameState.Interaction)
            Else
                SetState(GameState.Navigation)
            End If
        End With
    End Sub
End Class
