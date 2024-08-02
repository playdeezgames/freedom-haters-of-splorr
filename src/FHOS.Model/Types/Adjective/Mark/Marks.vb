Friend Module Marks
    Friend ReadOnly MarkI As String = NameOf(MarkI)
    Friend ReadOnly MarkII As String = NameOf(MarkII)
    Friend ReadOnly MarkIII As String = NameOf(MarkIII)
    Friend ReadOnly MarkIV As String = NameOf(MarkIV)
    Friend ReadOnly MarkV As String = NameOf(MarkV)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, MarkDescriptor) =
        New List(Of MarkDescriptor) From
        {
            New MarkDescriptor(MarkI, 1, "Mark I", "An entry-level product for the value conscious."),
            New MarkDescriptor(MarkII, 2, "Mark II", "The perfect trade off between inexpensive and reliable."),
            New MarkDescriptor(MarkIII, 3, "Mark III", "The midrange model, with enhanced efficient and durability."),
            New MarkDescriptor(MarkIV, 4, "Mark IV", "For the serious product buyer, for whom average is just not good enough."),
            New MarkDescriptor(MarkV, 5, "Mark V", "For the discerning product purchaser, who settles for nothing but the best.")
        }.ToDictionary(Function(x) x.Mark, Function(x) x)
End Module
