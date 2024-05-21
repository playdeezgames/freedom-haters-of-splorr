Friend Module Grid5x5
    Friend ReadOnly C1R1 As String = NameOf(C1R1)
    Friend ReadOnly C1R2 As String = NameOf(C1R2)
    Friend ReadOnly C1R3 As String = NameOf(C1R3)
    Friend ReadOnly C1R4 As String = NameOf(C1R4)
    Friend ReadOnly C1R5 As String = NameOf(C1R5)
    Friend ReadOnly C2R1 As String = NameOf(C2R1)
    Friend ReadOnly C2R2 As String = NameOf(C2R2)
    Friend ReadOnly C2R3 As String = NameOf(C2R3)
    Friend ReadOnly C2R4 As String = NameOf(C2R4)
    Friend ReadOnly C2R5 As String = NameOf(C2R5)
    Friend ReadOnly C3R1 As String = NameOf(C3R1)
    Friend ReadOnly C3R2 As String = NameOf(C3R2)
    Friend ReadOnly C3R3 As String = NameOf(C3R3)
    Friend ReadOnly C3R4 As String = NameOf(C3R4)
    Friend ReadOnly C3R5 As String = NameOf(C3R5)
    Friend ReadOnly C4R1 As String = NameOf(C4R1)
    Friend ReadOnly C4R2 As String = NameOf(C4R2)
    Friend ReadOnly C4R3 As String = NameOf(C4R3)
    Friend ReadOnly C4R4 As String = NameOf(C4R4)
    Friend ReadOnly C4R5 As String = NameOf(C4R5)
    Friend ReadOnly C5R1 As String = NameOf(C5R1)
    Friend ReadOnly C5R2 As String = NameOf(C5R2)
    Friend ReadOnly C5R3 As String = NameOf(C5R3)
    Friend ReadOnly C5R4 As String = NameOf(C5R4)
    Friend ReadOnly C5R5 As String = NameOf(C5R5)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, Grid5x5Descriptor) =
        New List(Of Grid5x5Descriptor) From
        {
            New Grid5x5Descriptor(C1R1, ChrW(236)),
            New Grid5x5Descriptor(C2R1, ChrW(237)),
            New Grid5x5Descriptor(C3R1, ChrW(230)),
            New Grid5x5Descriptor(C4R1, ChrW(238)),
            New Grid5x5Descriptor(C5R1, ChrW(239)),
            New Grid5x5Descriptor(C1R2, ChrW(240)),
            New Grid5x5Descriptor(C2R2, ChrW(230)),
            New Grid5x5Descriptor(C3R2, ChrW(230)),
            New Grid5x5Descriptor(C4R2, ChrW(230)),
            New Grid5x5Descriptor(C5R2, ChrW(241)),
            New Grid5x5Descriptor(C1R3, ChrW(230)),
            New Grid5x5Descriptor(C2R3, ChrW(230)),
            New Grid5x5Descriptor(C3R3, ChrW(230)),
            New Grid5x5Descriptor(C4R3, ChrW(230)),
            New Grid5x5Descriptor(C5R3, ChrW(232)),
            New Grid5x5Descriptor(C1R4, ChrW(242)),
            New Grid5x5Descriptor(C2R4, ChrW(230)),
            New Grid5x5Descriptor(C3R4, ChrW(230)),
            New Grid5x5Descriptor(C4R4, ChrW(230)),
            New Grid5x5Descriptor(C5R4, ChrW(243)),
            New Grid5x5Descriptor(C1R5, ChrW(244)),
            New Grid5x5Descriptor(C2R5, ChrW(245)),
            New Grid5x5Descriptor(C3R5, ChrW(234)),
            New Grid5x5Descriptor(C4R5, ChrW(246)),
            New Grid5x5Descriptor(C5R5, ChrW(247))
        }.ToDictionary(Function(x) x.SectionName, Function(x) x)
End Module
