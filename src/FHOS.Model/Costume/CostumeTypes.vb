Friend Module CostumeTypes
    Friend ReadOnly MerchantVessel As String = NameOf(MerchantVessel)
    Friend ReadOnly MilitaryVessel As String = NameOf(MilitaryVessel)
    Friend ReadOnly Person As String = NameOf(Person)
    Friend ReadOnly TradingPost As String = NameOf(TradingPost)
    Friend ReadOnly StarDock As String = NameOf(StarDock)
    Friend ReadOnly Shipyard As String = NameOf(Shipyard)
    Friend ReadOnly StarGate As String = NameOf(StarGate)
    Friend ReadOnly Debris As String = NameOf(Debris)
    Friend ReadOnly Wormhole As String = NameOf(Wormhole)

    Friend Function MakePlanetVicinity(planetType As String) As String
        Return MakeCostume($"{planetType}PlanetVicinity", PlanetTypes.Descriptors(planetType).Hue)
    End Function

    Friend Function MakeCostume(costumeType As String, hue As Integer) As String
        Return $"{costumeType}{hue}"
    End Function

    Friend Function MakeArrow(directionName As String) As String
        Return MakeCostume($"Arrow{directionName}", Hues.DarkGray)
    End Function

    Friend Function MakeSatellite(satelliteType As String) As String
        Dim descriptor = SatelliteTypes.Descriptors(satelliteType)
        Return MakeCostume($"{satelliteType}Moon", descriptor.Hue)
    End Function

    Private ReadOnly glyphsTable As IReadOnlyDictionary(Of String, Char()) =
        New Dictionary(Of String, Char()) From
        {
            {MilitaryVessel, {ChrW(132), ChrW(133), ChrW(134), ChrW(135)}},
            {MerchantVessel, {ChrW(128), ChrW(129), ChrW(130), ChrW(131)}},
            {Person, {ChrW(160), ChrW(160), ChrW(160), ChrW(160)}},
            {TradingPost, {ChrW(248), ChrW(248), ChrW(248), ChrW(248)}},
            {StarDock, {ChrW(249), ChrW(249), ChrW(249), ChrW(249)}},
            {Shipyard, {ChrW(250), ChrW(250), ChrW(250), ChrW(250)}},
            {StarGate, {ChrW(251), ChrW(251), ChrW(251), ChrW(251)}},
            {Debris, {ChrW(252), ChrW(252), ChrW(252), ChrW(252)}},
            {Wormhole, {ChrW(228), ChrW(228), ChrW(228), ChrW(228)}}
        }

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, CostumeTypeDescriptor) =
        GenerateDescriptors().
        ToDictionary(Function(x) x.Name, Function(x) x)

    Private Function GenerateDescriptors() As IReadOnlyList(Of CostumeTypeDescriptor)
        Dim result = New List(Of CostumeTypeDescriptor)
        For Each hue In Hues.Descriptors.Keys
            For Each glyph In glyphsTable
                result.Add(New CostumeTypeDescriptor(MakeCostume(glyph.Key, hue), glyph.Value, hue))
            Next
        Next
        For Each ordinalDirection In OrdinalDirections.Descriptors
            result.Add(New CostumeTypeDescriptor(
                               MakeArrow(ordinalDirection.Key),
                               Enumerable.Range(0, 4).Select(Function(x) ordinalDirection.Value.Glyph).ToArray,
                               Hues.DarkGray))
        Next
        For Each satelliteType In SatelliteTypes.Descriptors
            result.Add(
                New CostumeTypeDescriptor(
                       MakeSatellite(satelliteType.Key),
                       Enumerable.Range(0, 4).Select(Function(x) ChrW(226)).ToArray,
                       satelliteType.Value.Hue))
        Next
        For Each planetType In PlanetTypes.Descriptors
            result.Add(
                New CostumeTypeDescriptor(
                       MakePlanetVicinity(planetType.Key),
                       Enumerable.Range(0, 4).Select(Function(x) ChrW(226)).ToArray,
                       planetType.Value.Hue))
            For Each section In Grid3x3.Descriptors
                result.Add(
                    New CostumeTypeDescriptor(
                        MakePlanetSection(planetType.Key, section.Key),
                        Enumerable.Range(0, 4).Select(Function(x) section.Value.Glyph).ToArray,
                        planetType.Value.Hue))
            Next
        Next
        Return result
    End Function

    Friend Function MakePlanetSection(planetType As String, sectionName As String) As String
        Dim descriptor = PlanetTypes.Descriptors(planetType)
        Return MakeCostume($"{planetType}Planet{sectionName}", descriptor.Hue)
    End Function
End Module
