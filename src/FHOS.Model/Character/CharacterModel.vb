Friend Class CharacterModel
    Implements ICharacterModel

    Private ReadOnly character As Persistence.IActor

    Public Sub New(character As Persistence.IActor)
        Me.character = character
    End Sub

    Public ReadOnly Property Glyph As Char Implements ICharacterModel.Glyph
        Get
            Return CharacterTypes.Descriptors(character.ActorType).Glyphs(character.Facing)
        End Get
    End Property

    Public ReadOnly Property Hue As Integer Implements ICharacterModel.Hue
        Get
            Return CharacterTypes.Descriptors(character.ActorType).Hue
        End Get
    End Property
End Class
