﻿Imports FHOS.Model
Imports SPLORR.UI

Friend Class ChangeGalacticDensityState
    Inherits BasePickerState(Of IWorldModel, String)

    Const NeverMindText = "NeverMind"

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of IWorldModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Galactic Density",
            context.ControlsText(aButton:="Choose", bButton:="Cancel"),
            BoilerplateState.Embark)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        Select Case value.Item
            Case NeverMindText
                SetState(BoilerplateState.Embark)
            Case Else
                Context.Model.SetGalacticDensity(value.Item)
                SetState(BoilerplateState.Embark)
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        Dim result As New List(Of (Text As String, Item As String)) From
            {
                ("No Change", NeverMindText)
            }
        result.AddRange(Context.Model.GalacticDensityOptions)
        Return result
    End Function
End Class