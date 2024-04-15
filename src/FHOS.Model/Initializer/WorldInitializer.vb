Imports FHOS.Persistence
Imports SPLORR.Game

Friend Module WorldInitializer
    Const StarMapColumns = 63
    Const StarMapRows = 63
    Const StarMapName = "Star Map"
    Const SystemMapColumns = 31
    Const SystemMapRows = 31

    Sub Initialize(world As IWorld, galacticAge As String, galacticDensity As String)
        Dim starMap = InitializeStarMap(world, galacticAge, galacticDensity)
        world.Avatar = InitializeCharacter(starMap)
    End Sub

    Private Function InitializeCharacter(map As IMap) As ICharacter
        Dim characterCell As ICell
        Dim descriptor = CharacterTypes.Descriptors(Player)
        Do
            characterCell = map.GetCell(RNG.FromRange(0, StarMapColumns - 1), RNG.FromRange(0, StarMapRows - 1))
        Loop Until descriptor.CanSpawn(characterCell)
        Dim character = map.World.CreateCharacter(Player, characterCell)
        character.MaximumOxygen = descriptor.MaximumOxygen
        character.Oxygen = descriptor.MaximumOxygen
        Return character
    End Function

    Private Function InitializeStarMap(world As IWorld, galacticAge As String, galacticDensity As String) As IMap
        Dim starMap = world.CreateMap(MapTypes.Stellar, StarMapName, StarMapColumns, StarMapRows, TerrainTypes.Void)
        Dim stars As New List(Of (Column As Integer, Row As Integer))
        Dim starSystemNames As New HashSet(Of String)
        Dim tries As Integer = 0
        Const MaximumTries = 5000
        Dim MinimumDistance = GalacticDensities.Descriptors(galacticDensity).MinimumDistance
        While tries < MaximumTries
            Dim column = RNG.FromRange(0, StarMapColumns - 1)
            Dim row = RNG.FromRange(0, StarMapRows - 1)
            If stars.All(Function(star) (column - star.Column) * (column - star.Column) + (row - star.Row) * (row - star.Row) >= MinimumDistance * MinimumDistance) Then
                Dim starType = GalacticAges.Descriptors(galacticAge).GenerateStarType()
                stars.Add((column, row))
                Dim cell = starMap.GetCell(column, row)
                cell.TerrainType = StarTypes.Descriptors(starType).TerrainType
                Dim starSystemName As String = GenerateUnusedStarSystemName(starSystemNames)
                cell.StarSystem = world.CreateStarSystem(starSystemName, starType)
                InitializeStarSystem(cell.StarSystem)
                tries = 0
            Else
                tries += 1
            End If
        End While
        Return starMap
    End Function

    Private Sub InitializeStarSystem(starSystem As IStarSystem)
        starSystem.Map = starSystem.World.CreateMap(
            MapTypes.System,
            $"{starSystem.Name} System",
            SystemMapColumns,
            SystemMapRows,
            TerrainTypes.Void)
        'mark system edge
        'corners
        'left edge
        'right edge
        'top edge
        'bottom edge
        'create teleporter
        'place teleport along system edge
    End Sub

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
            "Antwerp",
            "Kai",
            "Polatarnia",
            "Mlideen",
            "Shuitan",
            "Ralatlan",
            "Liefzol"
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
            "Iota",
            "Kappa",
            "Lambda",
            "Mu",
            "Nu",
            "Xi",
            "Omicron",
            "Pi",
            "Rho",
            "Sigma",
            "Tau",
            "Upsilon",
            "Phi",
            "Chi",
            "Psi",
            "Omega"
        }


    Private Function GenerateStarSystemName() As String
        Return $"{RNG.FromEnumerable(ConstellationNames)} {RNG.FromEnumerable(GreekLetterNames)}"
    End Function
End Module
