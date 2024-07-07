Friend Module Marks
    Friend ReadOnly MarkI As String = NameOf(MarkI)
    Friend ReadOnly MarkII As String = NameOf(MarkII)
    Friend ReadOnly MarkIII As String = NameOf(MarkIII)
    Friend ReadOnly MarkIV As String = NameOf(MarkIV)
    Friend ReadOnly MarkV As String = NameOf(MarkV)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, MarkDescriptor) =
        New List(Of MarkDescriptor) From
        {
            New MarkDescriptor(MarkI, "Mark I", "This is the oniest of all."),
            New MarkDescriptor(MarkII, "Mark II", "This is twoier than the Mark I."),
            New MarkDescriptor(MarkIII, "Mark III", "Not as twoey as the Mark II, but neither is it as fourey as the Mark IV."),
            New MarkDescriptor(MarkIV, "Mark IV", "Fourier than the Mark III. Not as fivey as the Mark V."),
            New MarkDescriptor(MarkV, "Mark V", "This is the fiviest!")
        }.ToDictionary(Function(x) x.Mark, Function(x) x)
End Module
