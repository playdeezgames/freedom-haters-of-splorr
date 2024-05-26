Friend Module LocationTypes

    Friend ReadOnly Void As String = NameOf(Void)
    Friend ReadOnly Air As String = NameOf(Air)
    Friend ReadOnly Bulkhead As String = NameOf(Bulkhead)
    Friend ReadOnly PlanetVicinity As String = NameOf(PlanetVicinity)
    Friend ReadOnly StarSystem As String = NameOf(StarSystem)
    Friend ReadOnly StarVicinity As String = NameOf(StarVicinity)
    Friend ReadOnly Star As String = NameOf(Star)
    Friend ReadOnly Planet As String = NameOf(Planet)
    Friend ReadOnly Satellite As String = NameOf(Satellite)
    Friend ReadOnly PlanetAdjacent As String = NameOf(PlanetAdjacent)
    Friend ReadOnly Wormhole As String = NameOf(Wormhole)
    Friend ReadOnly ActorAdjacent As String = NameOf(ActorAdjacent)

    Private ReadOnly Open As String = NameOf(Open)
    Private ReadOnly Shut As String = NameOf(Shut)

    Friend Function MakeDoor(direction As String, doorState As Boolean) As String
        Return $"Door{If(doorState, Open, Shut)}{direction}"
    End Function

    Friend Function MakeVoidArrow(direction As String) As String
        Return $"{Void}{direction}Arrow"
    End Function
    Private Function GenerateLocationTypes() As IReadOnlyDictionary(Of String, LocationTypeDescriptor)
        Dim result = New Dictionary(Of String, LocationTypeDescriptor) From
        {
            {Void, New LocationTypeDescriptor("Empty Space", ChrW(0), DarkGray, Black, canPlaceWormhole:=True)},
            {PlanetAdjacent, New LocationTypeDescriptor("Empty Space", ChrW(0), DarkGray, Black)},
            {ActorAdjacent, New LocationTypeDescriptor("Empty Space", ChrW(0), DarkGray, Black)},
            {PlanetVicinity, New LocationTypeDescriptor("Planet Vicinity", ChrW(0), DarkGray, Black, canPlaceWormhole:=False, isEnterable:=False)},
            {Wormhole, New LocationTypeDescriptor("Wormhole", ChrW(0), DarkGray, Black)},
            {StarSystem, New LocationTypeDescriptor("Star System", ChrW(0), DarkGray, Black, canPlaceWormhole:=False, isEnterable:=False)},
            {StarVicinity, New LocationTypeDescriptor("Star Vicinity", ChrW(0), DarkGray, Black, canPlaceWormhole:=False, isEnterable:=False)},
            {Star, New LocationTypeDescriptor("Star", ChrW(0), DarkGray, Black, canPlaceWormhole:=False, isEnterable:=False)},
            {Planet, New LocationTypeDescriptor("Planet", ChrW(0), DarkGray, Black, canPlaceWormhole:=False, isEnterable:=False)},
            {Satellite, New LocationTypeDescriptor("Satellite", ChrW(0), DarkGray, Black, canPlaceWormhole:=False, isEnterable:=False)},
            {Air, New LocationTypeDescriptor("Air", ChrW(0), DarkGray, Black)},
            {Bulkhead, New LocationTypeDescriptor("Bulkhead", ChrW(230), DarkGray, Black, isEnterable:=False)},
            {MakeDoor(CardinalDirections.North, True), New LocationTypeDescriptor("Open Door", ChrW(24), Hues.DarkGray, Black)},
            {MakeDoor(CardinalDirections.North, False), New LocationTypeDescriptor("Shut Door", ChrW(25), Hues.DarkGray, Black, isEnterable:=False)},
            {MakeDoor(CardinalDirections.East, True), New LocationTypeDescriptor("Open Door", ChrW(26), Hues.DarkGray, Black)},
            {MakeDoor(CardinalDirections.East, False), New LocationTypeDescriptor("Shut Door", ChrW(27), Hues.DarkGray, Black, isEnterable:=False)},
            {MakeDoor(CardinalDirections.South, True), New LocationTypeDescriptor("Open Door", ChrW(28), Hues.DarkGray, Black)},
            {MakeDoor(CardinalDirections.South, False), New LocationTypeDescriptor("Shut Door", ChrW(29), Hues.DarkGray, Black, isEnterable:=False)},
            {MakeDoor(CardinalDirections.West, True), New LocationTypeDescriptor("Open Door", ChrW(30), Hues.DarkGray, Black)},
            {MakeDoor(CardinalDirections.West, False), New LocationTypeDescriptor("Shut Door", ChrW(31), Hues.DarkGray, Black, isEnterable:=False)}
        }
        For Each ordinalDirection In OrdinalDirections.Descriptors
            result(MakeVoidArrow(ordinalDirection.Key)) = New LocationTypeDescriptor("Empty Space", ordinalDirection.Value.Glyph, DarkGray, Black)
        Next
        Return result
    End Function

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, LocationTypeDescriptor) =
        GenerateLocationTypes()
End Module
