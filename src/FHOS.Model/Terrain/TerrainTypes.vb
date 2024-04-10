Friend Module TerrainTypes
    Friend Const Void = "Void"
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, TerrainDescriptor) =
        New Dictionary(Of String, TerrainDescriptor) From
        {
            {Void, New TerrainDescriptor("."c, 8, 0)}
        }
End Module
