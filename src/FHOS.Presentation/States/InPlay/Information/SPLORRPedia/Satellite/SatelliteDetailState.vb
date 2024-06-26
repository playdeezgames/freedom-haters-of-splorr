﻿Imports FHOS.Model
Imports SPLORR.UI

Friend Class SatelliteDetailState
    Inherits BaseGameState(Of IUniverseModel)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        Select Case cmd
            Case Command.B
                Context.Model.Ephemerals.SelectedSatellite.Pop()
                SetState(Nothing)
            Case Command.A
                SetState(GameState.SatelliteCrossReference)
        End Select
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim font = Context.Font(UIFontName)
        With Context.Model.Ephemerals.SelectedSatellite.Peek
            Context.ShowHeader(displayBuffer, font, .Name, Context.UIPalette.Header, Context.UIPalette.Background)
            Dim position = (Context.ViewCenter.X, font.Height)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Type: { .Properties.SatelliteTypeName}", Hues.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Planet: { .Parents.Planet.Name}", Hues.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Star System: { .Parents.StarSystem.Name}", Hues.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Faction: { .Parents.Planet.Parents.Faction.Name}", Hues.Black)
            Context.ShowStatusBar(
                displayBuffer,
                font,
                Context.
                ControlsText(aButton:="Go To...", bButton:="Go Back"), Context.UIPalette.Background, Context.UIPalette.Footer)
        End With
    End Sub

    Public Overrides Sub OnStart()
        MyBase.OnStart()
    End Sub
End Class
