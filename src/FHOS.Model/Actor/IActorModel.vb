Public Interface IActorModel
    ReadOnly Property Sprite As (Glyph As Char, Hue As Integer)
    ReadOnly Property Name As String
    ReadOnly Property Faction As IFactionModel
End Interface
