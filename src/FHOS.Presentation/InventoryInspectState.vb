Imports FHOS.Model
Imports SPLORR.UI

Friend Class InventoryInspectState
    Inherits BaseGameState(Of Model.IUniverseModel)

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(parent, setState, context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        SetState(GameState.Inventory)
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Hues.Cyan)
        Dim font = Context.Font(UIFont)
        With Context.Model.Ephemerals.InventoryItemStack
            Context.ShowHeader(displayBuffer, font, .ItemName, Context.UIPalette.Header, Context.UIPalette.Background)
            Dim position = (Context.ViewCenter.X, font.Height)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, .Description, Hues.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Quantity: { .Count}", Hues.Black)
            Context.ShowStatusBar(displayBuffer, font, Context.ControlsText(bButton:="Go Back"), Context.UIPalette.Background, Context.UIPalette.Footer)
        End With
    End Sub
End Class
