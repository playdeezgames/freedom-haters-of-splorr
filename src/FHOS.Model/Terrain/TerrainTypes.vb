Friend Module TerrainTypes
    Friend Const Void = "Void"
    Friend Const Star = "Star"
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, TerrainDescriptor) =
        New Dictionary(Of String, TerrainDescriptor) From
        {
            {Void, New TerrainDescriptor(ChrW(0), DarkGray, Black)},
            {Star, New TerrainDescriptor("*"c, LightGray, Black)}
        }
End Module
