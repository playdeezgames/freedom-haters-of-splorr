Imports FHOS.Model
Imports SPLORR.Presentation

Friend MustInherit Class BoardState
    Inherits BaseState
    Protected Const ViewColumns = 21
    Protected Const ViewRows = 21


    Private Shared ReadOnly glyphTable As IReadOnlyDictionary(Of Char, Char) =
        New Dictionary(Of Char, Char) From
        {
            {ChrW(0), "."c},
            {ChrW(16), "^"c},
            {ChrW(17), "/"c},
            {ChrW(18), ">"c},
            {ChrW(19), "\"c},
            {ChrW(20), "v"c},
            {ChrW(21), "/"c},
            {ChrW(22), "<"c},
            {ChrW(23), "\"c},
            {ChrW(24), "a"c},
            {ChrW(25), "b"c},
            {ChrW(26), "c"c},
            {ChrW(27), "d"c},
            {ChrW(28), "e"c},
            {ChrW(29), "f"c},
            {ChrW(30), "g"c},
            {ChrW(31), "h"c},
            {ChrW(128), "@"c},
            {ChrW(129), "@"c},
            {ChrW(130), "@"c},
            {ChrW(131), "@"c},
            {ChrW(132), "M"c},
            {ChrW(133), "M"c},
            {ChrW(134), "M"c},
            {ChrW(135), "M"c},
            {ChrW(136), "X"c},
            {ChrW(137), "X"c},
            {ChrW(138), "X"c},
            {ChrW(139), "X"c},
            {ChrW(140), "X"c},
            {ChrW(141), "X"c},
            {ChrW(142), "X"c},
            {ChrW(143), "X"c},
            {ChrW(144), "X"c},
            {ChrW(145), "X"c},
            {ChrW(146), "X"c},
            {ChrW(147), "X"c},
            {ChrW(148), "X"c},
            {ChrW(149), "X"c},
            {ChrW(150), "X"c},
            {ChrW(151), "X"c},
            {ChrW(152), "X"c},
            {ChrW(153), "X"c},
            {ChrW(154), "X"c},
            {ChrW(155), "X"c},
            {ChrW(156), "X"c},
            {ChrW(157), "X"c},
            {ChrW(158), "X"c},
            {ChrW(159), "X"c},
            {ChrW(160), "X"c},
            {ChrW(161), "X"c},
            {ChrW(162), "X"c},
            {ChrW(163), "X"c},
            {ChrW(164), "X"c},
            {ChrW(165), "X"c},
            {ChrW(166), "X"c},
            {ChrW(167), "X"c},
            {ChrW(168), "X"c},
            {ChrW(169), "X"c},
            {ChrW(170), "X"c},
            {ChrW(171), "X"c},
            {ChrW(172), "X"c},
            {ChrW(173), "X"c},
            {ChrW(174), "X"c},
            {ChrW(175), "X"c},
            {ChrW(176), "X"c},
            {ChrW(177), "X"c},
            {ChrW(178), "X"c},
            {ChrW(179), "X"c},
            {ChrW(180), "X"c},
            {ChrW(181), "X"c},
            {ChrW(182), "X"c},
            {ChrW(183), "X"c},
            {ChrW(184), "X"c},
            {ChrW(185), "X"c},
            {ChrW(186), "X"c},
            {ChrW(187), "X"c},
            {ChrW(188), "X"c},
            {ChrW(189), "X"c},
            {ChrW(190), "X"c},
            {ChrW(191), "X"c},
            {ChrW(192), "X"c},
            {ChrW(193), "X"c},
            {ChrW(194), "X"c},
            {ChrW(195), "X"c},
            {ChrW(196), "X"c},
            {ChrW(197), "X"c},
            {ChrW(198), "X"c},
            {ChrW(199), "X"c},
            {ChrW(200), "X"c},
            {ChrW(201), "X"c},
            {ChrW(202), "X"c},
            {ChrW(203), "X"c},
            {ChrW(204), "X"c},
            {ChrW(205), "X"c},
            {ChrW(206), "X"c},
            {ChrW(207), "X"c},
            {ChrW(208), "X"c},
            {ChrW(209), "X"c},
            {ChrW(210), "X"c},
            {ChrW(211), "X"c},
            {ChrW(212), "X"c},
            {ChrW(213), "X"c},
            {ChrW(214), "X"c},
            {ChrW(215), "X"c},
            {ChrW(216), "X"c},
            {ChrW(217), "X"c},
            {ChrW(218), "X"c},
            {ChrW(219), "X"c},
            {ChrW(220), "X"c},
            {ChrW(221), "X"c},
            {ChrW(222), "X"c},
            {ChrW(223), "X"c},
            {ChrW(224), "*"c},
            {ChrW(225), "O"c},
            {ChrW(226), "o"c},
            {ChrW(227), "°"c},
            {ChrW(228), "§"c},
            {ChrW(229), "#"c},
            {ChrW(230), "#"c},
            {ChrW(231), "#"c},
            {ChrW(232), "#"c},
            {ChrW(233), "#"c},
            {ChrW(234), "#"c},
            {ChrW(235), "#"c},
            {ChrW(236), "#"c},
            {ChrW(237), "#"c},
            {ChrW(238), "#"c},
            {ChrW(239), "#"c},
            {ChrW(240), "#"c},
            {ChrW(241), "#"c},
            {ChrW(242), "#"c},
            {ChrW(243), "#"c},
            {ChrW(244), "#"c},
            {ChrW(245), "#"c},
            {ChrW(246), "#"c},
            {ChrW(247), "#"c},
            {ChrW(248), "T"c},
            {ChrW(249), "+"c},
            {ChrW(250), "Y"c},
            {ChrW(251), "0"c},
            {ChrW(252), "%"c},
            {ChrW(253), "X"c},
            {ChrW(254), "X"c},
            {ChrW(255), "X"c}
        }

    Protected Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub
    Protected Const MinimumRow = -ViewRows \ 2
    Protected Const MaximumRow = ViewRows \ 2
    Protected Const MinimumColumn = -ViewColumns \ 2
    Protected Const MaximumColumn = ViewColumns \ 2
    Protected Sub RenderBoard(Optional cursor As (X As Integer, Y As Integer)? = Nothing)
        For Each boardY In Enumerable.Range(MinimumRow, ViewRows)
            For Each boardX In Enumerable.Range(MinimumColumn, ViewColumns)
                Dim locationModel = model.State.GetLocation((boardX, boardY))

                If locationModel.Exists Then
                    Dim glyph = If(locationModel.Actor?.Sprite.Glyph, locationModel.Sprite.Glyph)
                    Dim foreground = If(locationModel.Actor?.Sprite.Hue, locationModel.Sprite.Foreground)
                    If cursor.HasValue AndAlso cursor.Value.X = boardX AndAlso cursor.Value.Y = boardY Then
                        ui.Write((Mood.Pink, $"["))
                        ui.Write((moodTable(foreground), $"{glyphTable(glyph)}"))
                        ui.Write((Mood.Pink, $"]"))
                    Else
                        ui.Write((moodTable(foreground), $" {glyphTable(glyph)} "))
                    End If
                Else
                    ui.Write((Mood.Cyan, "..."))
                End If
            Next
            ui.WriteLine((Mood.Black, String.Empty))
        Next
    End Sub
End Class
