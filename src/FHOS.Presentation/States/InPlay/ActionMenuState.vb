﻿Imports SPLORR.UI

Friend Class ActionMenuState
    Inherits BasePickerState(Of Model.IWorldModel, String)
    Private Const EnterStarSystemText = "Enter Star System"

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IWorldModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Action Menu",
            context.ControlsText(aButton:="Choose", bButton:="Cancel"),
            BoilerplateState.Neutral)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        Select Case value.Item
            Case GoBackText
                SetState(BoilerplateState.Neutral)
            Case EnterStarSystemText
                SetState(GameState.EnterStarSystem)
            Case Else
                Throw New NotImplementedException
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        Dim result As New List(Of (Text As String, Item As String)) From
            {
                (GoBackText, GoBackText)
            }
        If Context.Model.Avatar.CanEnterStarSystem Then
            result.Add((EnterStarSystemText, EnterStarSystemText))
        End If
        Return result
    End Function
End Class