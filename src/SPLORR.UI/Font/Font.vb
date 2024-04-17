Public Class Font
    Private ReadOnly _glyphs As New Dictionary(Of Char, GlyphBuffer)
    Public ReadOnly Height As Integer
    Public ReadOnly Property HalfHeight As Integer
        Get
            Return Height \ 2
        End Get
    End Property
    Public Sub New(fontData As FontData)
        Height = fontData.Height
        For Each glyph In fontData.Glyphs.Keys
            _glyphs(glyph) = New GlyphBuffer(fontData, glyph)
        Next
    End Sub
    Public Sub WriteLeftText(sink As IPixelSink, position As (X As Integer, Y As Integer), text As String, hue As Integer)
        If text Is Nothing Then
            Return
        End If
        WriteText(sink, position, text, hue)
    End Sub
    Public Sub WriteRightText(sink As IPixelSink, position As (X As Integer, Y As Integer), text As String, hue As Integer)
        If text Is Nothing Then
            Return
        End If
        WriteText(sink, (position.X - TextWidth(text), position.Y), text, hue)
    End Sub
    Private Sub WriteText(sink As IPixelSink, position As (X As Integer, Y As Integer), text As String, hue As Integer)
        For Each character In text
            Dim buffer = _glyphs(character)
            buffer.CopyTo(sink, position, hue)
            position = (position.X + buffer.Width, position.Y)
        Next
    End Sub
    Public Sub WriteCenteredText(sink As IPixelSink, position As (X As Integer, Y As Integer), text As String, hue As Integer)
        If text Is Nothing Then
            Return
        End If
        WriteText(sink, (position.X - HalfTextWidth(text), position.Y), text, hue)
    End Sub
    Public Function TextWidth(text As String) As Integer
        Return text.Select(Function(character) _glyphs(character).Width).Sum
    End Function
    Public Function HalfTextWidth(text As String) As Integer
        Return TextWidth(text) \ 2
    End Function
End Class
