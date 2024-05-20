Friend Module LocationTypes

    Friend ReadOnly Void As String = NameOf(Void)
    Friend ReadOnly Air As String = NameOf(Air)
    Friend ReadOnly Bulkhead As String = NameOf(Bulkhead)
    Friend ReadOnly Door As String = NameOf(Door)
    Friend ReadOnly Open As String = NameOf(Open)
    Friend ReadOnly Shut As String = NameOf(Shut)
    Friend ReadOnly Star As String = NameOf(Star)
    Friend ReadOnly Planet As String = NameOf(Planet)
    Friend ReadOnly Moon As String = NameOf(Moon)

    Friend Function MakeDoor(direction As String, doorState As String) As String
        Return $"{Door}{doorState}{direction}"
    End Function

    Friend Function MakeVoidArrow(direction As String) As String
        Return $"{Void}{direction}"
    End Function

    Friend Function MakeStar(starType As String) As String
        Return $"{starType}{Star}"
    End Function

    Friend ReadOnly BlueWhiteStar As String = NameOf(BlueWhiteStar)
    Friend ReadOnly YellowStar As String = NameOf(YellowStar)
    Friend ReadOnly OrangeStar As String = NameOf(OrangeStar)
    Friend ReadOnly RedStar As String = NameOf(RedStar)



    Friend Function MakePlanetLocationType(planetType As String) As String
        Return $"{planetType}{Planet}"
    End Function
    Friend Function MakeSatelliteLocationType(satelliteType As String) As String
        Return $"{satelliteType}{Moon}"
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
            {SatelliteTypes.Radiated, Hues.Cyan},
            {SatelliteTypes.Volcanic, Hues.Orange},
            {SatelliteTypes.Barren, Hues.DarkGray},
            {SatelliteTypes.Inferno, Hues.Red},
            {SatelliteTypes.Cavernous, Hues.LightGray},
            {SatelliteTypes.Ice, Hues.White}
        }

    Friend ReadOnly WormholeExit As String = NameOf(WormholeExit)

    Private ReadOnly planetHueTable As IReadOnlyDictionary(Of String, Integer) =
        New Dictionary(Of String, Integer) From
        {
            {PlanetTypes.Radiated, Hues.Yellow},
            {Toxic, Hues.Purple},
            {PlanetTypes.Volcanic, Hues.Orange},
            {PlanetTypes.Barren, Hues.DarkGray},
            {Desert, Hues.Brown},
            {Tundra, Hues.White},
            {Arid, Hues.Tan},
            {Ocean, Hues.Blue},
            {Terran, Hues.Green},
            {PlanetTypes.Inferno, Hues.Red},
            {Tropical, Hues.LightBlue},
            {Grassland, Hues.LightGreen},
            {PlanetTypes.Cavernous, Hues.LightGray},
            {Gaia, Hues.Pink},
            {Swamp, Hues.Cyan}
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
            {Air, New LocationTypeDescriptor("Air", ChrW(0), DarkGray, Black)},
            {Bulkhead, New LocationTypeDescriptor("Bulkhead", ChrW(230), DarkGray, Black, isEnterable:=False)},
            {MakeVoidArrow(OrdinalDirections.North), New LocationTypeDescriptor("Empty Space", ChrW(16), DarkGray, Black)},
            {MakeVoidArrow(OrdinalDirections.NorthEast), New LocationTypeDescriptor("Empty Space", ChrW(17), DarkGray, Black)},
            {MakeVoidArrow(OrdinalDirections.East), New LocationTypeDescriptor("Empty Space", ChrW(18), DarkGray, Black)},
            {MakeVoidArrow(OrdinalDirections.SouthEast), New LocationTypeDescriptor("Empty Space", ChrW(19), DarkGray, Black)},
            {MakeVoidArrow(OrdinalDirections.South), New LocationTypeDescriptor("Empty Space", ChrW(20), DarkGray, Black)},
            {MakeVoidArrow(OrdinalDirections.SouthWest), New LocationTypeDescriptor("Empty Space", ChrW(21), DarkGray, Black)},
            {MakeVoidArrow(OrdinalDirections.West), New LocationTypeDescriptor("Empty Space", ChrW(22), DarkGray, Black)},
            {MakeVoidArrow(OrdinalDirections.NorthWest), New LocationTypeDescriptor("Empty Space", ChrW(23), DarkGray, Black)},
            {MakeStar(StarTypes.Blue), New LocationTypeDescriptor("Blue Star", ChrW(224), Hues.Blue, Black)},
            {MakeStar(StarTypes.BlueWhite), New LocationTypeDescriptor("Blue-White Star", ChrW(224), Hues.LightBlue, Black)},
            {MakeStar(StarTypes.Yellow), New LocationTypeDescriptor("Yellow Star", ChrW(224), Hues.Yellow, Black)},
            {MakeStar(StarTypes.Orange), New LocationTypeDescriptor("Orange Star", ChrW(224), Hues.Orange, Black)},
            {MakeStar(StarTypes.Red), New LocationTypeDescriptor("Red Star", ChrW(224), Hues.Red, Black)},
            {WormholeExit, New LocationTypeDescriptor("Wormhole Exit", ChrW(0), Hues.DarkGray, Hues.Black)},
            {MakeDoor(CardinalDirections.North, Open), New LocationTypeDescriptor("Open Door", ChrW(24), Hues.DarkGray, Black)},
            {MakeDoor(CardinalDirections.North, Shut), New LocationTypeDescriptor("Shut Door", ChrW(25), Hues.DarkGray, Black, isEnterable:=False)},
            {MakeDoor(CardinalDirections.East, Open), New LocationTypeDescriptor("Open Door", ChrW(26), Hues.DarkGray, Black)},
            {MakeDoor(CardinalDirections.East, Shut), New LocationTypeDescriptor("Shut Door", ChrW(27), Hues.DarkGray, Black, isEnterable:=False)},
            {MakeDoor(CardinalDirections.South, Open), New LocationTypeDescriptor("Open Door", ChrW(28), Hues.DarkGray, Black)},
            {MakeDoor(CardinalDirections.South, Shut), New LocationTypeDescriptor("Shut Door", ChrW(29), Hues.DarkGray, Black, isEnterable:=False)},
            {MakeDoor(CardinalDirections.West, Open), New LocationTypeDescriptor("Open Door", ChrW(30), Hues.DarkGray, Black)},
            {MakeDoor(CardinalDirections.West, Shut), New LocationTypeDescriptor("Shut Door", ChrW(31), Hues.DarkGray, Black, isEnterable:=False)}
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
