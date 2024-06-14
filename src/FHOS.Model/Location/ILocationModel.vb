Public Interface ILocationModel
    ReadOnly Property Exists As Boolean
    ReadOnly Property Actor As IActorModel
    ReadOnly Property HasActor As Boolean
    ReadOnly Property Sprite As (Glyph As Char, Foreground As Integer, Background As Integer)
    ReadOnly Property Name As String
End Interface
