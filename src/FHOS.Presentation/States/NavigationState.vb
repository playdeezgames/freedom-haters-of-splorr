Imports FHOS.Model
Imports SPLORR.UI

Friend Class NavigationState
    Inherits BaseGameState(Of IWorldModel)

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of IWorldModel))
        MyBase.New(parent, setState, context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        Select Case cmd
            Case Command.Start, Command.B
                SetState(BoilerplateState.GameMenu)
        End Select
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim board = Context.Model.Board
        For Each boardColumn In board
            Dim x = PlotX(boardColumn.Key)
            For Each boardRow In boardColumn.Value
                Dim y = PlotY(boardRow.Key)
                DrawTerrain(displayBuffer, x, y, boardRow.Value.TerrainType)
                DrawCharacter(displayBuffer, x, y, boardRow.Value.Character)
            Next
        Next
        Context.ShowStatusBar(displayBuffer, Context.Font(UIFontName), Context.ControlsText(Nothing, "Game Menu"), Context.UIPalette.Background, Context.UIPalette.Footer)
    End Sub

    Private Sub DrawCharacter(displayBuffer As IPixelSink, x As Integer, y As Integer, character As (CharacterType As String, Facing As Integer))
        If String.IsNullOrEmpty(character.CharacterType) Then
            Return
        End If
        For Each glyph In Glyphs.CharacterType(character.CharacterType)
            Context.Font(glyph.FontName).WriteText(displayBuffer, (x, y), ChrW(glyph.Indices(character.Facing)), glyph.Hue)
        Next
    End Sub

    Private Sub DrawTerrain(displayBuffer As IPixelSink, x As Integer, y As Integer, terrainType As String)
        If String.IsNullOrEmpty(terrainType) Then
            Return
        End If
        For Each glyph In Glyphs.TerrainType(terrainType)
            Context.Font(glyph.FontName).WriteText(displayBuffer, (x, y), ChrW(glyph.Index), glyph.Hue)
        Next
    End Sub

    Private Const CellWidth = 12
    Private Const CellHeight = 12
    Private Function PlotX(column As Integer) As Integer
        Return ViewWidth \ 2 - CellWidth \ 2 + column * CellWidth
    End Function

    Private Function PlotY(row As Integer) As Integer
        Return ViewHeight \ 2 - CellHeight \ 2 - Context.Font(UIFontName).HalfHeight + row * CellHeight
    End Function
End Class
