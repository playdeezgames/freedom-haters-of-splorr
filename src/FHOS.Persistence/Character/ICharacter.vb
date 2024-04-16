Public Interface ICharacter
    ReadOnly Property Id As Integer
    Property Cell As ICell
    ReadOnly Property CharacterType As String
    Property MaximumOxygen As Integer
    Property Oxygen As Integer
    Property Facing As Integer
    ReadOnly Property HasTutorial As Boolean
    ReadOnly Property CurrentTutorial As String
    Sub DismissTutorial()
End Interface
