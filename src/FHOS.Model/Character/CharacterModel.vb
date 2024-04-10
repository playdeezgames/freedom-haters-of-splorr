Friend Class CharacterModel
    Implements ICharacterModel

    Private character As Persistence.ICharacter

    Public Sub New(character As Persistence.ICharacter)
        Me.character = character
    End Sub
End Class
