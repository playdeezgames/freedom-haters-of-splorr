Imports FHOS.Persistence
Imports SPLORR.Game

Friend Module WorldInitializer
    Const StarMapColumns = 63
    Const StarMapRows = 63
    Const CharacterTypePlayer = "Player"
    Const TerrainTypeVoid = "Void"

    Sub Initialize(world As IWorld)
        Dim starMap = InitializeStarMap(world)
        Dim character = InitializeCharacter(starMap)
        world.SetAvatar(character)
    End Sub

    Private Function InitializeCharacter(map As IMap) As ICharacter
        Dim character = map.World.CreateCharacter(CharacterTypePlayer, map.GetCell(RNG.FromRange(0, StarMapColumns - 1), RNG.FromRange(0, StarMapRows - 1)))
        Return character
    End Function

    Private Function InitializeStarMap(world As IWorld) As IMap
        Dim starMap = world.CreateMap(StarMapColumns, StarMapRows, TerrainTypeVoid)
        Return starMap
    End Function
End Module
