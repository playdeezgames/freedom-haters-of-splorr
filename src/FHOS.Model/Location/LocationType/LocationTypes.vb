Friend Module LocationTypes

    Friend ReadOnly Void As String = NameOf(Void)
    Friend ReadOnly Air As String = NameOf(Air)
    Friend ReadOnly Bulkhead As String = NameOf(Bulkhead)
    Private ReadOnly Open As String = NameOf(Open)
    Private ReadOnly Shut As String = NameOf(Shut)

    Friend Function MakeDoor(direction As String, doorState As Boolean) As String
        Return $"Door{If(doorState, Open, Shut)}{direction}"
    End Function

    Friend Function MakeVoidArrow(direction As String) As String
        Return $"{Void}{direction}Arrow"
    End Function

    Friend Function MakeStar(starType As String) As String
        Return $"{starType}Star"
    End Function

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
            {MakeDoor(CardinalDirections.North, True), New LocationTypeDescriptor("Open Door", ChrW(24), Hues.DarkGray, Black)},
            {MakeDoor(CardinalDirections.North, False), New LocationTypeDescriptor("Shut Door", ChrW(25), Hues.DarkGray, Black, isEnterable:=False)},
            {MakeDoor(CardinalDirections.East, True), New LocationTypeDescriptor("Open Door", ChrW(26), Hues.DarkGray, Black)},
            {MakeDoor(CardinalDirections.East, False), New LocationTypeDescriptor("Shut Door", ChrW(27), Hues.DarkGray, Black, isEnterable:=False)},
            {MakeDoor(CardinalDirections.South, True), New LocationTypeDescriptor("Open Door", ChrW(28), Hues.DarkGray, Black)},
            {MakeDoor(CardinalDirections.South, False), New LocationTypeDescriptor("Shut Door", ChrW(29), Hues.DarkGray, Black, isEnterable:=False)},
            {MakeDoor(CardinalDirections.West, True), New LocationTypeDescriptor("Open Door", ChrW(30), Hues.DarkGray, Black)},
            {MakeDoor(CardinalDirections.West, False), New LocationTypeDescriptor("Shut Door", ChrW(31), Hues.DarkGray, Black, isEnterable:=False)}
        }
        For Each satelliteHue In SatelliteTypes.Descriptors
            result(MakeSatelliteLocationType(satelliteHue.Key)) = New LocationTypeDescriptor($"{satelliteHue.Key} Moon", ChrW(226), satelliteHue.Value.Hue, Black)
            For Each satelliteSection In Grid3x3.Descriptors
                result(MakeSatelliteSectionLocationType(satelliteHue.Key, satelliteSection.Key)) = New LocationTypeDescriptor($"{satelliteHue.Key} Moon", satelliteSection.Value.Glyph, satelliteHue.Value.Hue, Black)
            Next
        Next
        For Each planetHue In PlanetTypes.Descriptors
            result(MakePlanetLocationType(planetHue.Key)) = New LocationTypeDescriptor($"{planetHue.Key} Planet", ChrW(225), planetHue.Value.Hue, Black)
            For Each planetSection In Grid3x3.Descriptors
                result(MakePlanetSectionLocationType(planetHue.Key, planetSection.Key)) = New LocationTypeDescriptor($"{planetHue.Key} Planet", planetSection.Value.Glyph, planetHue.Value.Hue, Black)
            Next
            For Each planetSection In Grid5x5.Descriptors
                result(MakePlanetSectionLocationType(planetHue.Key, planetSection.Key)) = New LocationTypeDescriptor($"{planetHue.Key} Planet", planetSection.Value.Glyph, planetHue.Value.Hue, Black)
            Next
        Next
        Return result
    End Function

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, LocationTypeDescriptor) =
        GenerateLocationTypes()
End Module
