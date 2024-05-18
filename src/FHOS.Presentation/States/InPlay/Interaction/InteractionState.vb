Imports FHOS.Model
Imports SPLORR.UI

Friend Class InteractionState
    Inherits BasePickerState(Of Model.IUniverseModel, IInteractionModel)

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

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As IInteractionModel))
        value.Item.Perform()
        SetState(BoilerplateState.Neutral)
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As IInteractionModel))
        Return Context.Model.State.Avatar.Interaction.AvailableChoices
    End Function

    Protected Overrides Function GetCenterY() As Integer
        Return Context.ViewSize.Height * 7 \ 8
    End Function

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        MyBase.Render(displayBuffer)
        displayBuffer.Fill((0, 0), (Context.ViewSize.Width, Context.ViewSize.Height * 3 \ 4), Context.UIPalette.Background)
        Dim position = (Context.ViewCenter.X, 0)
        Dim uiFont = Context.Font(UIFontName)
        For Each line In Context.Model.State.Avatar.Interaction.Lines
            position = uiFont.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, line.Text, line.Hue)
        Next
    End Sub
End Class
