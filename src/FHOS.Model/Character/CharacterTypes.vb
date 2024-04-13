Friend Module CharacterTypes
    Friend Const Player = "Player"
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, CharacterTypeDescriptor) =
        New Dictionary(Of String, CharacterTypeDescriptor) From
        {
            {
                Player,
                New CharacterTypeDescriptor(
                    {ChrW(128), ChrW(129), ChrW(130), ChrW(131)},
                    Hue.LightGray,
                    100,
                    Function(x) x.TerrainType = TerrainTypes.Void)}
        }
End Module
