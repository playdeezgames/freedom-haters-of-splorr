Friend Module TerrainTypes
    Friend Const Void = "Void"
    Friend Const Star = "Star"
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, TerrainDescriptor) =
        New Dictionary(Of String, TerrainDescriptor) From
        {
            {Void, New TerrainDescriptor("."c, 8, 0)},
            {Star, New TerrainDescriptor("*"c, 7, 0)}
        }
End Module
