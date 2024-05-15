Imports SPLORR.UI

Friend Class DoVerbState
    Inherits BaseGameState(Of Model.IUniverseModel)
    Private ReadOnly verbType As String

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel),
                  verbType As String)
        MyBase.New(parent, setState, context)
        Me.verbType = verbType
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub OnStart()
        MyBase.OnStart()
        Context.Model.Avatar.Verbs.Perform(verbType)
        SetState(BoilerplateState.Neutral)
    End Sub
End Class
