﻿Imports FHOS.Model
Imports SPLORR.UI

Friend Class ChangeGalacticAgeState
    Inherits BasePickerState(Of IUniverseModel, String)

    Const NeverMindText = "NeverMind"

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Galactic Age",
            context.ChooseOrCancel,
            BoilerplateState.Embark)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        Select Case value.Item
            Case NeverMindText
                SetState(BoilerplateState.Embark)
            Case Else
                Context.Model.Settings.GalacticAge.SetAge(value.Item)
                SetState(BoilerplateState.Embark)
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        Dim result As New List(Of (Text As String, Item As String)) From
            {
                ("No Change", NeverMindText)
            }
        result.AddRange(
            Context.Model.Settings.GalacticAge.Options.Select(
                Function(x) (If(x.Item = Context.Model.Settings.GalacticAge.Current, $"** {x.Text} **", x.Text), x.Item)))
        Return result
    End Function
End Class
