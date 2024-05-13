Friend Module LocationTypes
    Friend ReadOnly Void As String = NameOf(Void)
    Friend ReadOnly VoidNorthArrow As String = NameOf(VoidNorthArrow)
    Friend ReadOnly VoidNorthEastArrow As String = NameOf(VoidNorthEastArrow)
    Friend ReadOnly VoidEastArrow As String = NameOf(VoidEastArrow)
    Friend ReadOnly VoidSouthEastArrow As String = NameOf(VoidSouthEastArrow)
    Friend ReadOnly VoidSouthArrow As String = NameOf(VoidSouthArrow)
    Friend ReadOnly VoidSouthWestArrow As String = NameOf(VoidSouthWestArrow)
    Friend ReadOnly VoidWestArrow As String = NameOf(VoidWestArrow)
    Friend ReadOnly VoidNorthWestArrow As String = NameOf(VoidNorthWestArrow)

    Friend ReadOnly BlueStar As String = NameOf(BlueStar)
    Friend ReadOnly BlueWhiteStar As String = NameOf(BlueWhiteStar)
    Friend ReadOnly YellowStar As String = NameOf(YellowStar)
    Friend ReadOnly OrangeStar As String = NameOf(OrangeStar)
    Friend ReadOnly RedStar As String = NameOf(RedStar)

    Friend ReadOnly TopLeft As String = NameOf(TopLeft)
    Friend ReadOnly TopCenter As String = NameOf(TopCenter)
    Friend ReadOnly TopRight As String = NameOf(TopRight)
    Friend ReadOnly CenterLeft As String = NameOf(CenterLeft)
    Friend ReadOnly Center As String = NameOf(Center)
    Friend ReadOnly CenterRight As String = NameOf(CenterRight)
    Friend ReadOnly BottomLeft As String = NameOf(BottomLeft)
    Friend ReadOnly BottomCenter As String = NameOf(BottomCenter)
    Friend ReadOnly BottomRight As String = NameOf(BottomRight)


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

    Friend ReadOnly Wormhole As String = NameOf(Wormhole)

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
    Friend ReadOnly C1R1 As String = NameOf(C1R1)
    Friend ReadOnly C1R2 As String = NameOf(C1R2)
    Friend ReadOnly C1R3 As String = NameOf(C1R3)
    Friend ReadOnly C1R4 As String = NameOf(C1R4)
    Friend ReadOnly C1R5 As String = NameOf(C1R5)
    Friend ReadOnly C2R1 As String = NameOf(C2R1)
    Friend ReadOnly C2R2 As String = NameOf(C2R2)
    Friend ReadOnly C2R3 As String = NameOf(C2R3)
    Friend ReadOnly C2R4 As String = NameOf(C2R4)
    Friend ReadOnly C2R5 As String = NameOf(C2R5)
    Friend ReadOnly C3R1 As String = NameOf(C3R1)
    Friend ReadOnly C3R2 As String = NameOf(C3R2)
    Friend ReadOnly C3R3 As String = NameOf(C3R3)
    Friend ReadOnly C3R4 As String = NameOf(C3R4)
    Friend ReadOnly C3R5 As String = NameOf(C3R5)
    Friend ReadOnly C4R1 As String = NameOf(C4R1)
    Friend ReadOnly C4R2 As String = NameOf(C4R2)
    Friend ReadOnly C4R3 As String = NameOf(C4R3)
    Friend ReadOnly C4R4 As String = NameOf(C4R4)
    Friend ReadOnly C4R5 As String = NameOf(C4R5)
    Friend ReadOnly C5R1 As String = NameOf(C5R1)
    Friend ReadOnly C5R2 As String = NameOf(C5R2)
    Friend ReadOnly C5R3 As String = NameOf(C5R3)
    Friend ReadOnly C5R4 As String = NameOf(C5R4)
    Friend ReadOnly C5R5 As String = NameOf(C5R5)
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
