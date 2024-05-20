Friend Module Grid3x3
    Friend ReadOnly TopLeft As String = NameOf(TopLeft)
    Friend ReadOnly TopCenter As String = NameOf(TopCenter)
    Friend ReadOnly TopRight As String = NameOf(TopRight)
    Friend ReadOnly CenterLeft As String = NameOf(CenterLeft)
    Friend ReadOnly Center As String = NameOf(Center)
    Friend ReadOnly CenterRight As String = NameOf(CenterRight)
    Friend ReadOnly BottomLeft As String = NameOf(BottomLeft)
    Friend ReadOnly BottomCenter As String = NameOf(BottomCenter)
    Friend ReadOnly BottomRight As String = NameOf(BottomRight)

    Friend ReadOnly All As IReadOnlyList(Of String) =
        New List(Of String) From
        {
            TopLeft,
            TopCenter,
            TopRight,
            CenterLeft,
            Center,
            CenterRight,
            BottomLeft,
            BottomCenter,
            BottomRight
        }
End Module
