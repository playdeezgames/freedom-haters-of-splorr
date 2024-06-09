Friend Module CategoryTypes
    Friend ReadOnly Satellite As String = NameOf(Satellite)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, CategoryTypeDescriptor) =
        New List(Of CategoryTypeDescriptor) From
        {
            New SatelliteCategoryTypeDescriptor
        }.ToDictionary(Function(x) x.CategoryType, Function(x) x)
End Module
