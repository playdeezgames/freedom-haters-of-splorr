Public Interface IActorModel
    ReadOnly Property Sprite As (Glyph As Char, Hue As Integer)
    ReadOnly Property Name As String
    ReadOnly Property Group As IGroupModel
    ReadOnly Property Subtype As String
    ReadOnly Property IsStarSystem As Boolean
    ReadOnly Property Position As (X As Integer, Y As Integer)
End Interface
