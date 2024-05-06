Friend Module LocationTypes
    Friend Const Void = "Void"
    Friend Const VoidNorthArrow = "VoidNorthArrow"
    Friend Const VoidNorthEastArrow = "VoidNorthEastArrow"
    Friend Const VoidEastArrow = "VoidEastArrow"
    Friend Const VoidSouthEastArrow = "VoidSouthEastArrow"
    Friend Const VoidSouthArrow = "VoidSouthArrow"
    Friend Const VoidSouthWestArrow = "VoidSouthWestArrow"
    Friend Const VoidWestArrow = "VoidWestArrow"
    Friend Const VoidNorthWestArrow = "VoidNorthWestArrow"

    Friend Const BlueStar = "BlueStar"
    Friend Const BlueWhiteStar = "BlueWhiteStar"
    Friend Const YellowStar = "YellowStar"
    Friend Const OrangeStar = "OrangeStar"
    Friend Const RedStar = "RedStar"

    Friend Function MakePlanetLocationType(planetType As String) As String
        Return $"{planetType}Planet"
    End Function
    Friend Function MakePlanetSectionLocationType(planetType As String, sectionName As String) As String
        Return $"{MakePlanetLocationType(planetType)}{sectionName}"
    End Function

    Friend Const RadiatedMoon = "RadiatedMoon"
    Friend Const VolcanicMoon = "VolcanicMoon"
    Friend Const BarrenMoon = "BarrenMoon"
    Friend Const InfernoMoon = "InfernoMoon"
    Friend Const CavernousMoon = "CavernousMoon"
    Friend Const IceMoon = "IceMoon"

    Friend Const Wormhole = "Wormhole"

    Private ReadOnly planetHueTable As IReadOnlyDictionary(Of String, Integer) =
        New Dictionary(Of String, Integer) From
        {
            {Radiated, Hue.Yellow},
            {Toxic, Hue.Purple},
            {Volcanic, Hue.Orange},
            {Barren, Hue.DarkGray},
            {Desert, Hue.Brown},
            {Tundra, Hue.White},
            {Arid, Hue.Tan},
            {Ocean, Hue.Blue},
            {Terran, Hue.Green},
            {Inferno, Hue.Red},
            {Tropical, Hue.LightBlue},
            {Grassland, Hue.LightGreen},
            {Cavernous, Hue.LightGray},
            {Gaia, Hue.Pink},
            {Swamp, Hue.Cyan}
        }
    Friend Const TopLeft = "TopLeft"
    Friend Const TopCenter = "TopCenter"
    Friend Const TopRight = "TopRight"
    Friend Const CenterLeft = "CenterLeft"
    Friend Const Center = "Center"
    Friend Const CenterRight = "CenterRight"
    Friend Const BottomLeft = "BottomLeft"
    Friend Const BottomCenter = "BottomCenter"
    Friend Const BottomRight = "BottomRight"
    Private ReadOnly planetSectionTable As IReadOnlyDictionary(Of String, Char) =
        New Dictionary(Of String, Char) From
        {
            {TopLeft, ChrW(229)},
            {TopCenter, ChrW(230)},
            {TopRight, ChrW(231)},
            {CenterLeft, ChrW(230)},
            {Center, ChrW(230)},
            {CenterRight, ChrW(232)},
            {BottomLeft, ChrW(233)},
            {BottomCenter, ChrW(234)},
            {BottomRight, ChrW(235)}
        }
    Private Function GenerateLocationTypes() As IReadOnlyDictionary(Of String, LocationTypeDescriptor)
        Dim result = New Dictionary(Of String, LocationTypeDescriptor) From
        {
            {Void, New LocationTypeDescriptor("Empty Space", ChrW(0), DarkGray, Black, canPlaceWormhole:=True)},
            {VoidNorthArrow, New LocationTypeDescriptor("Empty Space", ChrW(16), DarkGray, Black)},
            {VoidNorthEastArrow, New LocationTypeDescriptor("Empty Space", ChrW(17), DarkGray, Black)},
            {VoidEastArrow, New LocationTypeDescriptor("Empty Space", ChrW(18), DarkGray, Black)},
            {VoidSouthEastArrow, New LocationTypeDescriptor("Empty Space", ChrW(19), DarkGray, Black)},
            {VoidSouthArrow, New LocationTypeDescriptor("Empty Space", ChrW(20), DarkGray, Black)},
            {VoidSouthWestArrow, New LocationTypeDescriptor("Empty Space", ChrW(21), DarkGray, Black)},
            {VoidWestArrow, New LocationTypeDescriptor("Empty Space", ChrW(22), DarkGray, Black)},
            {VoidNorthWestArrow, New LocationTypeDescriptor("Empty Space", ChrW(23), DarkGray, Black)},
            {BlueStar, New LocationTypeDescriptor("Blue Star", ChrW(224), Hue.Blue, Black)},
            {BlueWhiteStar, New LocationTypeDescriptor("Blue-White Star", ChrW(224), Hue.LightBlue, Black)},
            {YellowStar, New LocationTypeDescriptor("Yellow Star", ChrW(224), Hue.Yellow, Black)},
            {OrangeStar, New LocationTypeDescriptor("Orange Star", ChrW(224), Hue.Orange, Black)},
            {RedStar, New LocationTypeDescriptor("Red Star", ChrW(224), Hue.Red, Black)},
            {RadiatedMoon, New LocationTypeDescriptor("Radiated Moon", ChrW(226), Hue.Cyan, Black)},
            {VolcanicMoon, New LocationTypeDescriptor("Volcanic Moon", ChrW(226), Hue.Orange, Black)},
            {BarrenMoon, New LocationTypeDescriptor("Barren Moon", ChrW(226), Hue.DarkGray, Black)},
            {InfernoMoon, New LocationTypeDescriptor("Inferno Moon", ChrW(226), Hue.Red, Black)},
            {CavernousMoon, New LocationTypeDescriptor("Cavernous Moon", ChrW(226), Hue.LightGray, Black)},
            {IceMoon, New LocationTypeDescriptor("Ice Moon", ChrW(226), Hue.White, Black)},
            {Wormhole, New LocationTypeDescriptor("Wormhole", ChrW(228), Hue.Purple, Black)}
        }
        For Each planetHue In planetHueTable
            result(MakePlanetLocationType(planetHue.Key)) = New LocationTypeDescriptor($"{planetHue.Key} Planet", ChrW(225), planetHue.Value, Black)
            For Each planetSection In planetSectionTable
                result(MakePlanetSectionLocationType(planetHue.Key, planetSection.Key)) = New LocationTypeDescriptor($"{planetHue.Key} Planet", planetSection.Value, planetHue.Value, Black)
            Next
        Next
        Return result
    End Function

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, LocationTypeDescriptor) =
        GenerateLocationTypes()
End Module
