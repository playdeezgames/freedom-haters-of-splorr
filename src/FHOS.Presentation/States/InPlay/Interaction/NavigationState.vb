Imports FHOS.Model
Imports SPLORR.UI

Friend Class NavigationState
    Inherits BoardState

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of IUniverseModel))
        MyBase.New(parent, setState, context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        Select Case cmd
            Case Command.Start, Command.B
                SetState(BoilerplateState.GameMenu)
            Case Command.Up
                SetState(GameState.MoveUp)
            Case Command.Down
                SetState(GameState.MoveDown)
            Case Command.Right
                SetState(GameState.MoveRight)
            Case Command.Left
                SetState(GameState.MoveLeft)
            Case Command.Select
                SetState(GameState.Scanner)
            Case Command.A
                If Context.Model.Avatar.HasActions Then
                    SetState(GameState.ActionMenu)
                End If
        End Select
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim uiFont = Context.Font(UIFontName)
        Dim locationWidth = uiFont.TextWidth(ChrW(32))
        Dim locationHeight = uiFont.Height
        RenderBoard(
            displayBuffer,
            uiFont,
            (0, 0),
            (locationWidth, locationHeight))
        RenderStatistics(displayBuffer, uiFont, (ViewColumns * locationWidth, 0))
        Context.ShowStatusBar(
            displayBuffer,
            uiFont,
            Context.ControlsText(
                aButton:=If(Context.Model.Avatar.HasActions, "Actions", Nothing),
                selectButton:="Scanner"),
            Black,
            DarkGray)
    End Sub

    Private Sub RenderStatistics(displayBuffer As IPixelSink, uiFont As Font, position As (X As Integer, Y As Integer))
        Dim textWidth = Context.ViewSize.Width - position.X
        With Context.Model.Avatar
            position = uiFont.WriteLeftTextLines(displayBuffer, position, textWidth, $"NAV SCREEN", Purple)
            position = uiFont.WriteLeftTextLines(displayBuffer, position, textWidth, $"{ .MapName} ({ .X},{ .Y})", Black)
            position = uiFont.WriteLeftTextLines(displayBuffer, position, textWidth, $"Turn: { .Turn}", Black)
            position = uiFont.WriteLeftTextLines(displayBuffer, position, textWidth, $"Jools: { .Jools}", Black)
            position = uiFont.WriteLeftTextLines(displayBuffer, position, textWidth, $"O2: { .OxygenPercent}%", .OxygenHue)
            position = uiFont.WriteLeftTextLines(displayBuffer, position, textWidth, $"Fuel: { .FuelPercent}%", .FuelHue)
            If .StarSystem.Current IsNot Nothing Then
                position = uiFont.WriteLeftTextLines(displayBuffer, position, textWidth, .StarSystem.Current.Name, Black)
            End If
            If .StarVicinity.Current IsNot Nothing Then
                position = uiFont.WriteLeftTextLines(displayBuffer, position, textWidth, .StarVicinity.Current.Name, Black)
            End If
            If .PlanetVicinity.Current IsNot Nothing Then
                position = uiFont.WriteLeftTextLines(displayBuffer, position, textWidth, .PlanetVicinity.Current.Name, Black)
            End If
        End With
    End Sub
End Class
