Public MustInherit Class BasePixelSink
    Implements IPixelSink
    Public MustOverride Sub SetPixel(x As Integer, y As Integer, hue As Integer) Implements IPixelSink.SetPixel
    Public Sub Fill(
                   location As (X As Integer, Y As Integer),
                   size As (Width As Integer, Height As Integer),
                   hue As Integer) Implements IPixelSink.Fill
        For Each x In Enumerable.Range(0, size.Width)
            For Each y In Enumerable.Range(0, size.Height)
                SetPixel(location.X + x, location.Y + y, hue)
            Next
        Next
    End Sub

    Public Sub Colorize(
                       source As IPixelSource,
                       fromLocation As (X As Integer, Y As Integer),
                       toLocation As (X As Integer, Y As Integer),
                       size As (Width As Integer, Height As Integer),
                       xform As Func(Of Integer, Integer?)) Implements IPixelSink.Colorize
        For Each x In Enumerable.Range(0, size.Width)
            For Each y In Enumerable.Range(0, size.Height)
                Dim sourceHue = source.GetPixel(x + fromLocation.X, y + fromLocation.Y)
                Dim destinationHue As Integer? = xform(sourceHue)
                If destinationHue IsNot Nothing Then
                    SetPixel(x + toLocation.X, y + toLocation.Y, destinationHue.Value)
                End If
            Next
        Next
    End Sub

    Public Sub Stretch(
                      source As IPixelSource,
                      fromLocation As (X As Integer, Y As Integer),
                      toLocation As (X As Integer, Y As Integer),
                      size As (Width As Integer, Height As Integer),
                      scale As (X As Integer, Y As Integer),
                      filter As Func(Of Integer, Boolean)) Implements IPixelSink.Stretch
        For Each x In Enumerable.Range(0, size.Width)
            For Each y In Enumerable.Range(0, size.Height)
                Dim hue = source.GetPixel(x + fromLocation.X, y + fromLocation.Y)
                If filter(hue) Then
                    Fill((x * scale.X + toLocation.X, y * scale.Y + toLocation.Y), scale, hue)
                End If
            Next
        Next
    End Sub

    Public Sub Frame(
                    location As (X As Integer, Y As Integer),
                    size As (Width As Integer, Height As Integer),
                    hue As Integer) Implements IPixelSink.Frame
        For Each x In Enumerable.Range(location.X, size.Width)
            SetPixel(x, location.Y, hue)
            SetPixel(x, location.Y + size.Height - 1, hue)
        Next
        For Each y In Enumerable.Range(location.Y + 1, size.Height - 2)
            SetPixel(location.X, y, hue)
            SetPixel(location.X + size.Width - 1, y, hue)
        Next
    End Sub

    Public MustOverride Sub Fill(hue As Integer) Implements IPixelSink.Fill
End Class
