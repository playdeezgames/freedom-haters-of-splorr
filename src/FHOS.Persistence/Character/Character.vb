Friend Class Character
    Inherits CharacterDataClient
    Implements ICharacter

    Public Sub New(worldData As Data.WorldData, characterId As Integer)
        MyBase.New(worldData, characterId)
    End Sub

    Public ReadOnly Property Id As Integer Implements ICharacter.Id
        Get
            Return CharacterId
        End Get
    End Property

    Public ReadOnly Property Cell As ICell Implements ICharacter.Cell
        Get
            Return New Cell(WorldData, CharacterData.CellId)
        End Get
    End Property

    Public ReadOnly Property CharacterType As String Implements ICharacter.CharacterType
        Get
            Return CharacterData.CharacterType
        End Get
    End Property

    Public ReadOnly Property Facing As Integer Implements ICharacter.Facing
        Get
            Return CharacterData.Facing
        End Get
    End Property
End Class
