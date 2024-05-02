Imports SPLORR.Game

Friend Module StarTypes
    Friend Const Blue = "Blue"
    Friend Const BlueWhite = "BlueWhite"
    Friend Const Yellow = "Yellow"
    Friend Const Orange = "Orange"
    Friend Const Red = "Red"
    Private ReadOnly defaultPlanetTypeGenerator As IReadOnlyDictionary(Of String, Integer) =
        New Dictionary(Of String, Integer) From
        {
            {PlanetTypes.Arid, 1},
            {PlanetTypes.Barren, 1},
            {PlanetTypes.Cavernous, 1},
            {PlanetTypes.Desert, 1},
            {PlanetTypes.Gaia, 1},
            {PlanetTypes.Grassland, 1},
            {PlanetTypes.Inferno, 1},
            {PlanetTypes.Ocean, 1},
            {PlanetTypes.Radiated, 1},
            {PlanetTypes.Swamp, 1},
            {PlanetTypes.Terran, 1},
            {PlanetTypes.Toxic, 1},
            {PlanetTypes.Tropical, 1},
            {PlanetTypes.Tundra, 1},
            {PlanetTypes.Volcanic, 1}
        }
    Private ReadOnly defaultPlanetCountGenerator As IReadOnlyDictionary(Of Integer, Integer) =
        New Dictionary(Of Integer, Integer) From
        {
            {2, 1},
            {3, 2},
            {4, 3},
            {5, 4},
            {6, 5},
            {7, 6},
            {8, 5},
            {9, 4},
            {10, 3},
            {11, 2},
            {12, 1}
        }
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, StarTypeDescriptor) =
        New Dictionary(Of String, StarTypeDescriptor) From
        {
            {
                StarTypes.Blue,
                New StarTypeDescriptor("Blue Star", LocationTypes.BlueStar, 1, 6, defaultPlanetTypeGenerator, defaultPlanetCountGenerator)
            },
            {
                StarTypes.BlueWhite,
                New StarTypeDescriptor("Blue-White Star", LocationTypes.BlueWhiteStar, 1, 8, defaultPlanetTypeGenerator, defaultPlanetCountGenerator)
            },
            {
                StarTypes.Yellow,
                New StarTypeDescriptor("Yellow Star", LocationTypes.YellowStar, 1, 10, defaultPlanetTypeGenerator, defaultPlanetCountGenerator)
            },
            {
                StarTypes.Orange,
                New StarTypeDescriptor("Orange Star", LocationTypes.OrangeStar, 1, 12, defaultPlanetTypeGenerator, defaultPlanetCountGenerator)
            },
            {
                StarTypes.Red, New StarTypeDescriptor("Red Star", LocationTypes.RedStar, 1, 14, defaultPlanetTypeGenerator, defaultPlanetCountGenerator)
            }
        }
    Private ReadOnly starTypeGenerator As IReadOnlyDictionary(Of String, Integer) =
        Descriptors.ToDictionary(Function(x) x.Key, Function(x) x.Value.GeneratorWeight)
    Friend Function GenerateStarType() As String
        Return RNG.FromGenerator(starTypeGenerator)
    End Function
End Module
