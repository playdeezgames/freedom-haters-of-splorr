﻿Imports FHOS.Model
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
        Dim oldX = target.X
        Dim oldY = target.Y
        target = (Math.Clamp(target.X + deltaX, 0, ViewColumns - 1), Math.Clamp(target.Y + deltaY, 0, ViewRows - 1))
        If Not TargetCell.Exists Then
            target = (oldX, oldY)
        End If
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
        uiFont.WriteLeftText(
            displayBuffer,
            (cellWidth * target.X, cellHeight * target.Y),
            ChrW(255),
            4)
        RenderDetails(displayBuffer, uiFont, (ViewColumns * cellWidth, 0))
        Context.ShowStatusBar(displayBuffer, uiFont, Context.ControlsText(selectButton:="Navigation"), Black, DarkGray)
    End Sub

    Private ReadOnly Property TargetCell As ICellModel
        Get
            Return Context.Model.Board.GetCell((target.X - ViewColumns \ 2, target.Y - ViewRows \ 2))
        End Get
    End Property

    Private Sub RenderDetails(displayBuffer As IPixelSink, uiFont As Font, position As (X As Integer, Y As Integer))
        If Not TargetCell.Exists Then
            Return
        End If
        uiFont.WriteLeftText(displayBuffer, position, TargetCell.Terrain.Name, Black)
        position = NextLine(position, uiFont)
        Dim starSystem = TargetCell.StarSystem
        If starSystem IsNot Nothing Then
            uiFont.WriteLeftText(displayBuffer, position, starSystem.Name, Black)
            position = NextLine(position, uiFont)
        End If
    End Sub

    Private Function NextLine(position As (X As Integer, Y As Integer), uiFont As Font) As (X As Integer, Y As Integer)
        Return (position.X, position.Y + uiFont.Height)
    End Function

    Public Overrides Sub OnStart()
        MyBase.OnStart()
        target = (ViewColumns \ 2, ViewRows \ 2)
    End Sub
End Class