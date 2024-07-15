Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class NavigationState
    Inherits BoardState
    Implements IState
    Protected Const ViewColumns = 21
    Protected Const ViewRows = 21
    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Private Shared ReadOnly moodTable As IReadOnlyDictionary(Of Integer, Mood) =
        New Dictionary(Of Integer, Mood) From
        {
            {Hues.Black, Mood.Black},
            {Hues.Blue, Mood.Blue},
            {Hues.Green, Mood.Green},
            {Hues.Cyan, Mood.Cyan},
            {Hues.Red, Mood.Red},
            {Hues.Purple, Mood.Purple},
            {Hues.Brown, Mood.Brown},
            {Hues.LightGray, Mood.LightGray},
            {Hues.DarkGray, Mood.DarkGray},
            {Hues.LightBlue, Mood.LightBlue},
            {Hues.LightGreen, Mood.LightGreen},
            {Hues.Tan, Mood.Tan},
            {Hues.Orange, Mood.Orange},
            {Hues.Pink, Mood.Pink},
            {Hues.Yellow, Mood.Yellow},
            {Hues.White, Mood.White}
        }

    Private Shared ReadOnly glyphTable As IReadOnlyDictionary(Of Char, Char) =
        New Dictionary(Of Char, Char) From
        {
            {ChrW(0), "."c},
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
            {ChrW(225), "X"c},
            {ChrW(226), "X"c},
            {ChrW(227), "X"c},
            {ChrW(228), "X"c},
            {ChrW(229), "X"c},
            {ChrW(230), "X"c},
            {ChrW(231), "X"c},
            {ChrW(232), "X"c},
            {ChrW(233), "X"c},
            {ChrW(234), "X"c},
            {ChrW(235), "X"c},
            {ChrW(236), "X"c},
            {ChrW(237), "X"c},
            {ChrW(238), "X"c},
            {ChrW(239), "X"c},
            {ChrW(240), "X"c},
            {ChrW(241), "X"c},
            {ChrW(242), "X"c},
            {ChrW(243), "X"c},
            {ChrW(244), "X"c},
            {ChrW(245), "X"c},
            {ChrW(246), "X"c},
            {ChrW(247), "X"c},
            {ChrW(248), "X"c},
            {ChrW(249), "X"c},
            {ChrW(250), "X"c},
            {ChrW(251), "X"c},
            {ChrW(252), "X"c},
            {ChrW(253), "X"c},
            {ChrW(254), "X"c},
            {ChrW(255), "X"c}
        }

    Public Overrides Function Run() As IState
        ui.Clear()
        For Each boardY In Enumerable.Range(-ViewRows \ 2, ViewRows)
            For Each boardX In Enumerable.Range(-ViewColumns \ 2, ViewColumns)
                Dim locationModel = model.State.GetLocation((boardX, boardY))

                If locationModel.Exists Then
                    Dim glyph = If(locationModel.Actor?.Sprite.Glyph, locationModel.Sprite.Glyph)
                    Dim foreground = If(locationModel.Actor?.Sprite.Hue, locationModel.Sprite.Background)
                    ui.Write((moodTable(foreground), $" {glyphTable(glyph)} "))
                Else
                    ui.Write((Mood.Cyan, "###"))
                End If
            Next
            ui.WriteLine((Mood.Black, String.Empty))
        Next
        ui.ReadKey()
        Return endState
    End Function
End Class
