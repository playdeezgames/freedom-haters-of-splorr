Friend Module CategoryTypes
    Friend ReadOnly PlanetVicinity As String = NameOf(PlanetVicinity)
    Friend ReadOnly Planet As String = NameOf(Planet)
    Friend ReadOnly Satellite As String = NameOf(Satellite)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, CategoryTypeDescriptor) =
        New List(Of CategoryTypeDescriptor) From
        {
            New PlanetVicinityCategoryTypeDescriptor,
            New PlanetCategoryTypeDescriptor,
            New SatelliteCategoryTypeDescriptor
        }.ToDictionary(Function(x) x.CategoryType, Function(x) x)
End Module
