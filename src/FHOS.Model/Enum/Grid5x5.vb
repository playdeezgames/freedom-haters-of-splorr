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

    Friend ReadOnly All As IReadOnlyList(Of String) =
        New List(Of String) From
        {
            C1R1,
            C1R2,
            C1R3,
            C1R4,
            C1R5,
            C2R1,
            C2R2,
            C2R3,
            C2R4,
            C2R5,
            C3R1,
            C3R2,
            C3R3,
            C3R4,
            C3R5,
            C4R1,
            C4R2,
            C4R3,
            C4R4,
            C4R5,
            C5R1,
            C5R2,
            C5R3,
            C5R5,
            C5R4
        }
End Module
