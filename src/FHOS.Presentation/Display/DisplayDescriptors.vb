Friend Module DisplayDescriptors
    Friend ReadOnly CharacterTypes As IReadOnlyDictionary(Of String, CharacterTypeDisplay) =
        New Dictionary(Of String, CharacterTypeDisplay) From
        {
            {Model.CharacterTypes.Player, New CharacterTypeDisplay()}
        }
    Friend ReadOnly TerrainTypes As IReadOnlyDictionary(Of String, TerrainTypeDisplay) =
        New Dictionary(Of String, TerrainTypeDisplay) From
        {
            {Model.TerrainTypes.Void, New TerrainTypeDisplay()}
        }
End Module
