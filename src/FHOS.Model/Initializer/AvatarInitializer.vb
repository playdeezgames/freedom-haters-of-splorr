Imports FHOS.Persistence
Imports SPLORR.Game

Friend Module AvatarInitializer
    Friend Function Initialize(map As IMap) As IActor
        Dim characterCell As ILocation
        Dim descriptor = CharacterTypes.Descriptors(Player)
        Do
            characterCell = map.GetLocation(RNG.FromRange(0, map.Size.Columns - 1), RNG.FromRange(0, map.Size.Rows - 1))
        Loop Until descriptor.CanSpawn(characterCell)
        Dim character = map.Universe.CreateCharacter(Player, characterCell)
        character.MaximumOxygen = descriptor.MaximumOxygen
        character.Oxygen = descriptor.MaximumOxygen
        Return character
    End Function
End Module
