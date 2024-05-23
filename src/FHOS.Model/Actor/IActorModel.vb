Public Interface IActorModel
    ReadOnly Property Sprite As (Glyph As Char, Hue As Integer)
    ReadOnly Property Name As String
    ReadOnly Property Group As IGroupModel
End Interface
