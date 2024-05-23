Imports FHOS.Model
Imports SPLORR.UI

Friend Class ScannerState
    Inherits BoardState
    Private target As (X As Integer, Y As Integer) = (ViewColumns \ 2, ViewRows \ 2)

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of IUniverseModel))
        MyBase.New(parent, setState, context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        Select Case cmd
            Case Command.Select, Command.B
                SetState(GameState.Navigation)
            Case Command.Up
                MoveTarget(0, -1)
            Case Command.Down
                MoveTarget(0, 1)
            Case Command.Left
                MoveTarget(-1, 0)
            Case Command.Right
                MoveTarget(1, 0)
        End Select
    End Sub

    Private Sub MoveTarget(deltaX As Integer, deltaY As Integer)
        Dim oldX = target.X
        Dim oldY = target.Y
        target = (Math.Clamp(target.X + deltaX, -ViewColumns \ 2, ViewColumns \ 2), Math.Clamp(target.Y + deltaY, -ViewRows \ 2, ViewRows \ 2))
        If Not TargetLocation.Exists Then
            target = (oldX, oldY)
        End If
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
        uiFont.WriteLeftText(
            displayBuffer,
            (locationWidth * (target.X + ViewColumns \ 2), locationHeight * (target.Y + ViewRows \ 2)),
            ChrW(255),
            4)
        RenderDetails(
            displayBuffer,
            uiFont,
            (ViewColumns * locationWidth, 0))
        Context.ShowStatusBar(
            displayBuffer,
            uiFont,
            Context.ControlsText(
                aButton:=If(TargetLocation.HasDetails, "Details", Nothing),
                selectButton:="Navigation"),
            Black,
            DarkGray)
    End Sub

    Private ReadOnly Property TargetLocation As ILocationModel
        Get
            Return Context.Model.State.Board.GetLocation(target)
        End Get
    End Property

    Private Sub RenderDetails(displayBuffer As IPixelSink, uiFont As Font, position As (X As Integer, Y As Integer))
        If Not TargetLocation.Exists Then
            Return
        End If
        Dim textWidth = Context.ViewSize.Width - position.X
        position = uiFont.WriteLeftTextLines(displayBuffer, position, textWidth, TargetLocation.LocationType.Name, Black)
        Dim actor = TargetLocation.Actor
        If actor IsNot Nothing Then
            position = uiFont.WriteLeftTextLines(displayBuffer, position, textWidth, $"{actor.Name}", Black)
        End If
    End Sub

    Public Overrides Sub OnStart()
        MyBase.OnStart()
        target = (0, 0)
    End Sub
End Class
