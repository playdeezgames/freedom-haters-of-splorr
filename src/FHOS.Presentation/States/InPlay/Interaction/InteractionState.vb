﻿Imports SPLORR.UI

Friend Class InteractionState
    Inherits BasePickerState(Of Model.IUniverseModel, String)
    Private Const GoodbyeText = "Good-bye"

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "",
            context.ControlsText(aButton:="Chooses", bButton:="Cancel"),
            GameState.LeaveInteraction)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        Select Case value.Item
            Case GoodbyeText
                SetState(GameState.LeaveInteraction)
            Case Else
                Throw New NotImplementedException
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        Return {
            (GoodbyeText, GoodbyeText)
            }.ToList
    End Function

    Protected Overrides Function GetCenterY() As Integer
        Return Context.ViewSize.Height * 7 \ 8
    End Function

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        MyBase.Render(displayBuffer)
        displayBuffer.Fill((0, 0), (Context.ViewSize.Width, Context.ViewSize.Height * 3 \ 4), Context.UIPalette.Background)
    End Sub
End Class