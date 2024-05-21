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

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, Grid3x3Descriptor) =
        New List(Of Grid3x3Descriptor) From
        {
            New Grid3x3Descriptor(TopLeft, ChrW(229), (-1, -1)),
            New Grid3x3Descriptor(TopCenter, ChrW(230), (0, -1)),
            New Grid3x3Descriptor(TopRight, ChrW(231), (1, -1)),
            New Grid3x3Descriptor(CenterLeft, ChrW(230), (-1, 0)),
            New Grid3x3Descriptor(Center, ChrW(230), (0, 0)),
            New Grid3x3Descriptor(CenterRight, ChrW(232), (1, 0)),
            New Grid3x3Descriptor(BottomLeft, ChrW(233), (-1, 1)),
            New Grid3x3Descriptor(BottomCenter, ChrW(234), (0, 1)),
            New Grid3x3Descriptor(BottomRight, ChrW(235), (1, 1))
        }.ToDictionary(Function(x) x.SectionName, Function(x) x)
End Module
