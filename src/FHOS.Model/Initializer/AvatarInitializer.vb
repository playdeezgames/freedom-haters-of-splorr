Imports FHOS.Persistence
Imports SPLORR.Game

Friend Module AvatarInitializer
    Friend Function Initialize(map As IMap) As ICharacter
        Dim characterCell As ICell
        Dim descriptor = CharacterTypes.Descriptors(Player)
        Do
            characterCell = map.GetCell(RNG.FromRange(0, map.Size.Columns - 1), RNG.FromRange(0, map.Size.Rows - 1))
        Loop Until descriptor.CanSpawn(characterCell)
        Dim character = map.World.CreateCharacter(Player, characterCell)
        character.MaximumOxygen = descriptor.MaximumOxygen
        character.Oxygen = descriptor.MaximumOxygen
        Return character
    End Function
End Module
