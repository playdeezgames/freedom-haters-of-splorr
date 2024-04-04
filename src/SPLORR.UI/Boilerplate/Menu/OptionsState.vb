Friend Class OptionsState(Of TModel)
    Inherits BasePickerState(Of TModel, String)
    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of TModel))
        MyBase.New(parent, setState, context, "Options", context.ControlsText("Choose", "Cancel"), Nothing)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        Select Case value.Item
            Case ToggleFullScreenText
                Parent.FullScreen = Not Parent.FullScreen
                SaveConfig()
            Case SetWindowSizeText
                SetState(BoilerplateState.WindowSize)
            Case SetSfxVolumeText
                SetState(BoilerplateState.SfxVolume)
            Case SetMuxVolumeText
                SetState(BoilerplateState.MuxVolume)
            Case Else
                Throw New NotImplementedException
        End Select
    End Sub
    Protected Overrides Function InitializeMenuItems() As List(Of (String, String))
        Return New List(Of (String, String)) From
            {
                (ToggleFullScreenText, ToggleFullScreenText),
                (SetWindowSizeText, SetWindowSizeText),
                (SetSfxVolumeText, SetSfxVolumeText),
                (SetMuxVolumeText, SetMuxVolumeText)
            }
    End Function
End Class
