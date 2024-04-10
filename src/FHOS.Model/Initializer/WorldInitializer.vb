Imports System.Data
Imports FHOS.Persistence
Imports SPLORR.Game

Friend Module WorldInitializer
    Const StarMapColumns = 63
    Const StarMapRows = 63

    Sub Initialize(world As IWorld)
        Dim starMap = InitializeStarMap(world)
        Dim character = InitializeCharacter(starMap)
        world.SetAvatar(character)
    End Sub

    Private Function InitializeCharacter(map As IMap) As ICharacter
        Dim character = map.World.CreateCharacter(Player, map.GetCell(RNG.FromRange(0, StarMapColumns - 1), RNG.FromRange(0, StarMapRows - 1)))
        Return character
    End Function

    Private Function InitializeStarMap(world As IWorld) As IMap
        Dim starMap = world.CreateMap(StarMapColumns, StarMapRows, TerrainTypes.Void)
        Dim stars As New List(Of (Column As Integer, Row As Integer))
        Dim tries As Integer = 0
        Const MaximumTries = 5000
        Const MinimumDistance = 10
        While tries < MaximumTries
            Dim column = RNG.FromRange(0, StarMapColumns - 1)
            Dim row = RNG.FromRange(0, StarMapRows - 1)
            If stars.All(Function(star) (column - star.Column) * (column - star.Column) + (row - star.Row) * (row - star.Row) >= MinimumDistance) Then
                Dim starType = StarTypes.GenerateStarType()
                stars.Add((column, row))
                starMap.GetCell(column, row).TerrainType = StarTypes.Descriptors(starType).TerrainType
                tries = 0
            Else
                tries += 1
            End If
        End While
        Return starMap
    End Function
End Module
