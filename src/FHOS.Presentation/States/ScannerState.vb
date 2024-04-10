Imports FHOS.Model
Imports SPLORR.UI

Friend Class ScannerState
    Inherits BoardState
    Private target As (X As Integer, Y As Integer) = (ViewColumns \ 2, ViewRows \ 2)

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of IWorldModel))
        MyBase.New(parent, setState, context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        Select Case cmd
            Case Command.Select
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
        target = (target.X + deltaX, target.Y + deltaY)
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim uiFont = Context.Font(UIFontName)
        Dim cellWidth = uiFont.TextWidth(ChrW(32))
        Dim cellHeight = uiFont.Height
        RenderBoard(
            displayBuffer,
            uiFont,
            (0, 0),
            (cellWidth, cellHeight))
        uiFont.WriteText(displayBuffer, (cellWidth * target.X, cellHeight * target.Y), ChrW(16), 4)
    End Sub
End Class
