Imports System.IO
Imports FHOS.Model
Imports SPLORR.UI

Public Class FHOSContext
    Inherits UIContext(Of IWorldModel)

    Public Sub New(fontFilenames As IReadOnlyDictionary(Of String, String), viewSize As (Integer, Integer))
        MyBase.New(New WorldModel, fontFilenames, viewSize, New FHOSUIPalette)
    End Sub
    Private ReadOnly multipliers As IReadOnlyList(Of Integer) =
        New List(Of Integer) From
        {
            3, 4, 5, 9, 10, 14, 15, 19, 20
        }

    Public Overrides ReadOnly Property AvailableWindowSizes As IEnumerable(Of (Integer, Integer))
        Get
            Return multipliers.Select(Function(multiplier) (ViewWidth * multiplier, ViewHeight * multiplier))
        End Get
    End Property
    Private ReadOnly DeltasAndColor As IReadOnlyList(Of (deltaX As Integer, deltaY As Integer, hue As Integer)) =
        New List(Of (Integer, Integer, Integer)) From
        {
            (-1, -1, 6),
            (0, -1, 6),
            (1, -1, 6),
            (-1, 0, 6),
            (1, 0, 6),
            (-1, 1, 6),
            (0, 1, 6),
            (1, 1, 6),
            (0, 0, 13)
        }
    Private Sub ShowTitle(displayBuffer As IPixelSink, font As Font)
        Dim text = GameTitle
        Dim x = ViewWidth \ 2 - font.TextWidth(text) \ 2
        Dim y = ViewHeight \ 2 - font.Height * 3 \ 2
        With font
            For Each deltaAndColor In DeltasAndColor
                .WriteText(
                    displayBuffer,
                    (x + deltaAndColor.deltaX, y + deltaAndColor.deltaY),
                    text,
                    deltaAndColor.hue)
            Next
        End With
    End Sub
    Public Overrides Sub ShowSplashContent(displayBuffer As IPixelSink, font As Font)
        ShowTitle(displayBuffer, font)
        ShowSubtitle(displayBuffer, font)
        ShowStatusBar(displayBuffer, font, ControlsText(ContinueText, Nothing), UIPalette.Background, UIPalette.Footer)
    End Sub

    Private Sub ShowSubtitle(displayBuffer As IPixelSink, font As Font)
        Dim text = GameSubtitle
        Dim x = ViewWidth \ 2 - font.TextWidth(text) \ 2
        Dim y = ViewHeight \ 2 + font.Height * 3 \ 2
        With font
            .WriteText(
                    displayBuffer,
                    (x, y),
                    text,
                    4)
        End With
    End Sub

    Private ReadOnly aboutLines As IDictionary(Of Integer, (Text As String, Hue As Integer)) =
        New Dictionary(Of Integer, (String, Integer)) From
        {
            {0, ("About Freedom Haters of SPLORR!!", 6)},
            {8, ("A Production of TheGrumpyGameDev", 0)},
            {12, ("See 'aboot.txt'", 0)}
        }

    Public Overrides Sub ShowAboutContent(displayBuffer As IPixelSink, font As Font)
        With font
            For Each aboutLine In aboutLines
                .WriteText(
                    displayBuffer,
                    (0, font.Height * aboutLine.Key),
                    aboutLine.Value.Text,
                    aboutLine.Value.Hue)
            Next
        End With
        ShowStatusBar(displayBuffer, font, ControlsText(Nothing, "Close"), UIPalette.Background, UIPalette.Footer)
    End Sub

    Public Overrides Sub AbandonGame()
        Model.Abandon()
    End Sub

    Public Overrides Sub LoadGame(slot As Integer)
        Model.Load(SlotFilename(slot))
    End Sub

    Public Overrides Sub SaveGame(slot As Integer)
        Model.Save(SlotFilename(slot))
    End Sub

    Public Overrides Function DoesSlotExist(slot As Integer) As Boolean
        Return File.Exists(SlotFilename(slot))
    End Function

    Private ReadOnly SlotFilename As IReadOnlyDictionary(Of Integer, String) =
        New Dictionary(Of Integer, String) From
        {
            {0, "scum.json"},
            {1, "slot1.json"},
            {2, "slot2.json"},
            {3, "slot3.json"},
            {4, "slot4.json"},
            {5, "slot5.json"}
        }
End Class
