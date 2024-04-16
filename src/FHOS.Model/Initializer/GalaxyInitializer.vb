Imports FHOS.Persistence
Imports SPLORR.Game

Friend Module GalaxyInitializer
    Private Const GalaxyColumns = 63
    Private Const GalaxyRows = 63
    Private Const GalaxyName = "Galaxy Map"

    Function Initialize(universe As IUniverse, galacticAge As String, galacticDensity As String) As IMap
        Dim starMap = universe.CreateMap(MapTypes.Stellar, GalaxyName, GalaxyColumns, GalaxyRows, TerrainTypes.Void)
        Dim stars As New List(Of (Column As Integer, Row As Integer))
        Dim starSystemNames As New HashSet(Of String)
        Dim tries As Integer = 0
        Const MaximumTries = 5000
        Dim MinimumDistance = GalacticDensities.Descriptors(galacticDensity).MinimumDistance
        While tries < MaximumTries
            Dim column = RNG.FromRange(0, GalaxyColumns - 1)
            Dim row = RNG.FromRange(0, GalaxyRows - 1)
            If stars.All(Function(star) (column - star.Column) * (column - star.Column) + (row - star.Row) * (row - star.Row) >= MinimumDistance * MinimumDistance) Then
                Dim starType = GalacticAges.Descriptors(galacticAge).GenerateStarType()
                stars.Add((column, row))
                Dim cell = starMap.GetCell(column, row)
                cell.TerrainType = StarTypes.Descriptors(starType).TerrainType
                cell.Tutorial = TutorialTypes.StarSystemEntry
                Dim starSystemName As String = GenerateUnusedStarSystemName(starSystemNames)
                cell.StarSystem = universe.CreateStarSystem(starSystemName, starType)
                StarSystemInitializer.Initialize(cell.StarSystem, cell, starSystemName)
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
