Public Interface IActorModel
    ReadOnly Property Sprite As (Glyph As Char, Hue As Integer)
    ReadOnly Property Name As String
    ReadOnly Property Group As IGroupModel
    ReadOnly Property Subtype As String
    ReadOnly Property IsStarSystem As Boolean
    ReadOnly Property IsPlanet As Boolean
    ReadOnly Property IsPlanetVicinity As Boolean
    ReadOnly Property IsSatellite As Boolean
    ReadOnly Property Position As (X As Integer, Y As Integer)
    ReadOnly Property PlanetCount As Integer
    ReadOnly Property SatelliteCount As Integer
    ReadOnly Property StarSystem As IGroupModel
    ReadOnly Property PlanetVicinity As IGroupModel
End Interface
