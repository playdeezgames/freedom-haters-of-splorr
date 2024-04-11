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

    Public Property Cell As ICell Implements ICharacter.Cell
        Get
            Return New Cell(WorldData, CharacterData.CellId)
        End Get
        Set(value As ICell)
            If value.Id <> CharacterData.CellId Then
                Cell.Character = Nothing
                CharacterData.CellId = value.Id
                value.Character = Me
            End If
        End Set
    End Property

    Public ReadOnly Property CharacterType As String Implements ICharacter.CharacterType
        Get
            Return CharacterData.CharacterType
        End Get
    End Property

    Public Property MaximumOxygen As Integer Implements ICharacter.MaximumOxygen
        Get
            Return Math.Max(0, CharacterData.MaximumOxygen)
        End Get
        Set(value As Integer)
            CharacterData.MaximumOxygen = Math.Max(0, value)
        End Set
    End Property

    Public Property Oxygen As Integer Implements ICharacter.Oxygen
        Get
            Return Math.Clamp(CharacterData.Oxygen, 0, MaximumOxygen)
        End Get
        Set(value As Integer)
            CharacterData.Oxygen = Math.Clamp(value, 0, MaximumOxygen)
        End Set
    End Property
End Class
