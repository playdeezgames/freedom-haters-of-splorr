Imports System.ComponentModel

Friend Class MuxVolumeState(Of TModel)
    Inherits BasePickerState(Of TModel, Single)
    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of TModel))
        MyBase.New(
            parent,
            setState,
            context,
            "<placeholder>",
            context.ControlsText(aButton:="Choose", bButton:="Cancel"),
            BoilerplateState.Options)
    End Sub
    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As Single))
        MuxVolume = value.Item
        PlayMux(MainTheme)
        SaveConfig()
        HeaderText = $"Mux Volume (Currently: {MuxVolume * 100:f0}%)"
    End Sub
    Protected Overrides Function InitializeMenuItems() As List(Of (String, Single))
        Dim currentVolume = $"{MuxVolume * 100:f0}%"
        HeaderText = $"Mux Volume (Currently: {currentVolume})"
        Dim result As New List(Of (String, Single))
        For index = 0 To 10
            Dim menuItemText = $"{index * 10}%"
            result.Add((menuItemText, index / 10.0F))
            If menuItemText = currentVolume Then
                MenuItemIndex = index
            End If
        Next
        Return result
    End Function
End Class
