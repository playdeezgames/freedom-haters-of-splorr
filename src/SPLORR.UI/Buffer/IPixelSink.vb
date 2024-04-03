Public Interface IPixelSink
    Sub SetPixel(
                x As Integer,
                y As Integer,
                hue As Integer)
    Sub Stretch(
            source As IPixelSource,
            fromLocation As (X As Integer, Y As Integer),
            toLocation As (X As Integer, Y As Integer),
            size As (Width As Integer, Height As Integer),
            scale As (X As Integer, Y As Integer),
            filter As Func(Of Integer, Boolean))
    Sub Colorize(
                source As IPixelSource,
                fromLocation As (X As Integer, Y As Integer),
                toLocation As (X As Integer, Y As Integer),
                size As (Width As Integer, Height As Integer),
                xform As Func(Of Integer, Integer?))
    Sub Fill(
            location As (X As Integer, Y As Integer),
            size As (Width As Integer, Height As Integer),
            hue As Integer)
    Sub Fill(hue As Integer)
    Sub Frame(
             location As (X As Integer, Y As Integer),
             size As (Width As Integer, Height As Integer),
             hue As Integer)
End Interface
