Imports System.Data
Imports FHOS.Persistence
Imports SPLORR.Game

Friend Module WorldInitializer
    Const StarMapColumns = 63
    Const StarMapRows = 63
    Const StarMapName = "Star Map"

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
        Dim starMap = world.CreateMap(StarMapName, StarMapColumns, StarMapRows, TerrainTypes.Void)
        Dim stars As New List(Of (Column As Integer, Row As Integer))
        Dim starSystemNames As New HashSet(Of String)
        Dim tries As Integer = 0
        Const MaximumTries = 5000
        Const MinimumDistance = 15
        While tries < MaximumTries
            Dim column = RNG.FromRange(0, StarMapColumns - 1)
            Dim row = RNG.FromRange(0, StarMapRows - 1)
            If stars.All(Function(star) (column - star.Column) * (column - star.Column) + (row - star.Row) * (row - star.Row) >= MinimumDistance) Then
                Dim starType = StarTypes.GenerateStarType()
                stars.Add((column, row))
                Dim cell = starMap.GetCell(column, row)
                cell.TerrainType = StarTypes.Descriptors(starType).TerrainType
                Dim starSystemName As String = GenerateUnusedStarSystemName(starSystemNames)
                cell.StarSystem = world.CreateStarSystem(starSystemName, starType)
                'TODO: generate system map
                tries = 0
            Else
                tries += 1
            End If
        End While
        Return starMap
    End Function

    Private Function GenerateUnusedStarSystemName(starSystemNames As HashSet(Of String)) As String
        Dim starSystemName As String
        Do
            starSystemName = GenerateStarSystemName()
        Loop Until Not starSystemNames.Contains(starSystemName)
        starSystemNames.Add(starSystemName)
        Return starSystemName
    End Function

    ReadOnly ConstellationNames As IReadOnlyList(Of String) = New List(Of String) From
        {
            "Thuspo",
            "Reylon",
            "Shyrana",
            "Larak",
            "Ywan",
            "Meleka",
            "Ardana",
            "Krala",
            "Kerdix",
            "Derna",
            "Zoora",
            "Treyok",
            "Neror",
            "Bejis",
            "Vulerai",
            "Tsanrua",
            "Grot",
            "Jhama",
            "Peshto",
            "Antwerp"
        }

    ReadOnly GreekLetterNames As IReadOnlyList(Of String) = New List(Of String) From
        {
            "Alpha",
            "Beta",
            "Gamma",
            "Delta",
            "Epsilon",
            "Zeta",
            "Eta",
            "Theta",
            "Kappa",
            "Lambda",
            "Mu",
            "Nu",
            "Xi",
            "Chi",
            "Omicron",
            "Pi",
            "Rho",
            "Sigma",
            "Tau",
            "Upsilon",
            "Phi",
            "Psi",
            "Omega"
        }


    Private Function GenerateStarSystemName() As String
        Return $"{RNG.FromEnumerable(ConstellationNames)} {RNG.FromEnumerable(GreekLetterNames)}"
    End Function
End Module
