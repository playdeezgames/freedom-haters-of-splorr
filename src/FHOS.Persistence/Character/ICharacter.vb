Public Interface ICharacter
    ReadOnly Property Id As Integer
    Property Cell As ICell
    ReadOnly Property CharacterType As String
    ReadOnly Property Facing As Integer
End Interface
