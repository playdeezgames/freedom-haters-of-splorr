Friend Module LocationTypes

    Friend ReadOnly Void As String = NameOf(Void)
    Friend ReadOnly PlanetVicinity As String = NameOf(PlanetVicinity)
    Friend ReadOnly StarSystem As String = NameOf(StarSystem)
    Friend ReadOnly StarVicinity As String = NameOf(StarVicinity)
    Friend ReadOnly Star As String = NameOf(Star)
    Friend ReadOnly Planet As String = NameOf(Planet)
    Friend ReadOnly Satellite As String = NameOf(Satellite)
    Friend ReadOnly PlanetAdjacent As String = NameOf(PlanetAdjacent)
    Friend ReadOnly Wormhole As String = NameOf(Wormhole)
    Friend ReadOnly ActorAdjacent As String = NameOf(ActorAdjacent)

    Friend Function MakeVoidArrow(direction As String) As String
        Return $"{Void}{direction}Arrow"
    End Function
    Private Function GenerateLocationTypes() As IReadOnlyDictionary(Of String, LocationTypeDescriptor)
        Dim result = New Dictionary(Of String, LocationTypeDescriptor) From
        {
            {Void, New LocationTypeDescriptor("Empty Space", ChrW(0), DarkGray, Black)},
            {PlanetAdjacent, New LocationTypeDescriptor("Empty Space", ChrW(0), DarkGray, Black)},
            {ActorAdjacent, New LocationTypeDescriptor("Empty Space", ChrW(0), DarkGray, Black)},
            {PlanetVicinity, New LocationTypeDescriptor("Planet Vicinity", ChrW(0), DarkGray, Black, isEnterable:=False)},
            {Wormhole, New LocationTypeDescriptor("Wormhole", ChrW(0), DarkGray, Black)},
            {StarSystem, New LocationTypeDescriptor("Star System", ChrW(0), DarkGray, Black, isEnterable:=False)},
            {StarVicinity, New LocationTypeDescriptor("Star Vicinity", ChrW(0), DarkGray, Black, isEnterable:=False)},
            {Star, New LocationTypeDescriptor("Star", ChrW(0), DarkGray, Black, isEnterable:=False)},
            {Planet, New LocationTypeDescriptor("Planet", ChrW(0), DarkGray, Black, isEnterable:=False)},
            {Satellite, New LocationTypeDescriptor("Satellite", ChrW(0), DarkGray, Black, isEnterable:=False)}
        }
        For Each ordinalDirection In OrdinalDirections.Descriptors
            result(MakeVoidArrow(ordinalDirection.Key)) = New LocationTypeDescriptor("Empty Space", ordinalDirection.Value.Glyph, DarkGray, Black)
        Next
        Return result
    End Function

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, LocationTypeDescriptor) =
        GenerateLocationTypes()
End Module
