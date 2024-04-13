Imports FHOS.Model
Imports SPLORR.UI

Friend Class MoveState
    Inherits BaseGameState(Of IWorldModel)

    Private ReadOnly delta As (X As Integer, Y As Integer)
    Private ReadOnly facing As Integer

    Public Sub New(
                  controller As FHOSController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of IWorldModel),
                  facing As Integer,
                  delta As (X As Integer, Y As Integer))
        MyBase.New(controller, setState, context)
        Me.delta = delta
        Me.facing = facing
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub OnStart()
        MyBase.OnStart()
        Context.Model.Avatar.SetFacing(facing)
        Context.Model.Avatar.Move(delta)
        SetState(BoilerplateState.Neutral)
    End Sub
End Class
