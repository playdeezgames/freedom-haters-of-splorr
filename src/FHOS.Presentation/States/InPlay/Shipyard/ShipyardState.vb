Imports FHOS.Model
Imports SPLORR.UI

Friend Class ShipyardState
    Inherits BasePickerState(Of IUniverseModel, String)
    Const LeaveText = "Leave"

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context, "<placeholder>",
            context.ChooseOrCancel,
            GameState.LeaveTrader)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        Select Case value.Item
            Case LeaveText
                SetState(GameState.LeaveShipyard)
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        With Model.State.Avatar.Shipyard
            HeaderText = .Specimen.Name
            Dim result As New List(Of (Text As String, Item As String)) From
                {
                    (LeaveText, LeaveText)
                }
            Return result
        End With
    End Function

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        MyBase.Render(displayBuffer)
        Dim font = Context.Font(UIFont)
        font.WriteCenteredText(displayBuffer, (Context.ViewCenter.X, font.Height), $"Jools: {Context.Model.State.Avatar.Jools}", DarkGray)
    End Sub
End Class
