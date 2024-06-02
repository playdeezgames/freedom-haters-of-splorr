Friend Module CategoryTypes
    Friend ReadOnly Faction As String = NameOf(Faction)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, CategoryTypeDescriptor) =
        New List(Of CategoryTypeDescriptor) From
        {
            New FactionCategoryTypeDescriptor
        }.ToDictionary(Function(x) x.CategoryType, Function(x) x)
End Module
