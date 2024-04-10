Friend Module CharacterTypes
    Friend Const Player = "Player"
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, CharacterDescriptor) =
        New Dictionary(Of String, CharacterDescriptor) From
        {
            {Player, New CharacterDescriptor(ChrW(128), Orange)}
        }
End Module
