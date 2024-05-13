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

    Friend Const TopLeft = "TopLeft"
    Friend Const TopCenter = "TopCenter"
    Friend Const TopRight = "TopRight"
    Friend Const CenterLeft = "CenterLeft"
    Friend Const Center = "Center"
    Friend Const CenterRight = "CenterRight"
    Friend Const BottomLeft = "BottomLeft"
    Friend Const BottomCenter = "BottomCenter"
    Friend Const BottomRight = "BottomRight"


    Friend Function MakePlanetLocationType(planetType As String) As String
        Return $"{planetType}Planet"
    End Function
    Friend Function MakeSatelliteLocationType(satelliteType As String) As String
        Return $"{satelliteType}Moon"
    End Function
    Friend Function MakePlanetSectionLocationType(planetType As String, sectionName As String) As String
        Return $"{MakePlanetLocationType(planetType)}{sectionName}"
    End Function
    Friend Function MakeSatelliteSectionLocationType(satelliteType As String, sectionName As String) As String
        Return $"{MakeSatelliteLocationType(satelliteType)}{sectionName}"
    End Function


    Private ReadOnly satelliteHueTable As IReadOnlyDictionary(Of String, Integer) =
        New Dictionary(Of String, Integer) From
        {
            {SatelliteTypes.Radiated, Hue.Cyan},
            {SatelliteTypes.Volcanic, Hue.Orange},
            {SatelliteTypes.Barren, Hue.DarkGray},
            {SatelliteTypes.Inferno, Hue.Red},
            {SatelliteTypes.Cavernous, Hue.LightGray},
            {SatelliteTypes.Ice, Hue.White}
        }

    Friend Const Wormhole = "Wormhole"

    Private ReadOnly planetHueTable As IReadOnlyDictionary(Of String, Integer) =
        New Dictionary(Of String, Integer) From
        {
            {PlanetTypes.Radiated, Hue.Yellow},
            {Toxic, Hue.Purple},
            {PlanetTypes.Volcanic, Hue.Orange},
            {PlanetTypes.Barren, Hue.DarkGray},
            {Desert, Hue.Brown},
            {Tundra, Hue.White},
            {Arid, Hue.Tan},
            {Ocean, Hue.Blue},
            {Terran, Hue.Green},
            {PlanetTypes.Inferno, Hue.Red},
            {Tropical, Hue.LightBlue},
            {Grassland, Hue.LightGreen},
            {PlanetTypes.Cavernous, Hue.LightGray},
            {Gaia, Hue.Pink},
            {Swamp, Hue.Cyan}
        }
    Private ReadOnly planet3x3SectionTable As IReadOnlyDictionary(Of String, Char) =
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
    Friend Const C1R1 = "C1R1"
    Friend Const C1R2 = "C1R2"
    Friend Const C1R3 = "C1R3"
    Friend Const C1R4 = "C1R4"
    Friend Const C1R5 = "C1R5"
    Friend Const C2R1 = "C2R1"
    Friend Const C2R2 = "C2R2"
    Friend Const C2R3 = "C2R3"
    Friend Const C2R4 = "C2R4"
    Friend Const C2R5 = "C2R5"
    Friend Const C3R1 = "C3R1"
    Friend Const C3R2 = "C3R2"
    Friend Const C3R3 = "C3R3"
    Friend Const C3R4 = "C3R4"
    Friend Const C3R5 = "C3R5"
    Friend Const C4R1 = "C4R1"
    Friend Const C4R2 = "C4R2"
    Friend Const C4R3 = "C4R3"
    Friend Const C4R4 = "C4R4"
    Friend Const C4R5 = "C4R5"
    Friend Const C5R1 = "C5R1"
    Friend Const C5R2 = "C5R2"
    Friend Const C5R3 = "C5R3"
    Friend Const C5R4 = "C5R4"
    Friend Const C5R5 = "C5R5"
    Private ReadOnly planet5x5SectionTable As IReadOnlyDictionary(Of String, Char) =
        New Dictionary(Of String, Char) From
        {
            {C1R1, ChrW(236)},
            {C2R1, ChrW(237)},
            {C3R1, ChrW(230)},
            {C4R1, ChrW(238)},
            {C5R1, ChrW(239)},
            {C1R2, ChrW(240)},
            {C2R2, ChrW(230)},
            {C3R2, ChrW(230)},
            {C4R2, ChrW(230)},
            {C5R2, ChrW(241)},
            {C1R3, ChrW(230)},
            {C2R3, ChrW(230)},
            {C3R3, ChrW(230)},
            {C4R3, ChrW(230)},
            {C5R3, ChrW(232)},
            {C1R4, ChrW(242)},
            {C2R4, ChrW(230)},
            {C3R4, ChrW(230)},
            {C4R4, ChrW(230)},
            {C5R4, ChrW(243)},
            {C1R5, ChrW(244)},
            {C2R5, ChrW(245)},
            {C3R5, ChrW(234)},
            {C4R5, ChrW(246)},
            {C5R5, ChrW(247)}
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
            {Wormhole, New LocationTypeDescriptor("Wormhole", ChrW(228), Hue.Purple, Black)}
        }
        For Each satelliteHue In satelliteHueTable
            result(MakeSatelliteLocationType(satelliteHue.Key)) = New LocationTypeDescriptor($"{satelliteHue.Key} Moon", ChrW(226), satelliteHue.Value, Black)
            For Each satelliteSection In planet3x3SectionTable
                result(MakeSatelliteSectionLocationType(satelliteHue.Key, satelliteSection.Key)) = New LocationTypeDescriptor($"{satelliteHue.Key} Moon", satelliteSection.Value, satelliteHue.Value, Black)
            Next
        Next
        For Each planetHue In planetHueTable
            result(MakePlanetLocationType(planetHue.Key)) = New LocationTypeDescriptor($"{planetHue.Key} Planet", ChrW(225), planetHue.Value, Black)
            For Each planetSection In planet3x3SectionTable
                result(MakePlanetSectionLocationType(planetHue.Key, planetSection.Key)) = New LocationTypeDescriptor($"{planetHue.Key} Planet", planetSection.Value, planetHue.Value, Black)
            Next
            For Each planetSection In planet5x5SectionTable
                result(MakePlanetSectionLocationType(planetHue.Key, planetSection.Key)) = New LocationTypeDescriptor($"{planetHue.Key} Planet", planetSection.Value, planetHue.Value, Black)
            Next
        Next
        Return result
    End Function

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, LocationTypeDescriptor) =
        GenerateLocationTypes()
End Module
