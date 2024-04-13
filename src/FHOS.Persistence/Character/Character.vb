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
            Return New Cell(WorldData, CharacterData.Statistics(StatisticTypes.CellId))
        End Get
        Set(value As ICell)
            If value.Id <> CharacterData.Statistics(StatisticTypes.CellId) Then
                Cell.Character = Nothing
                CharacterData.Statistics(StatisticTypes.CellId) = value.Id
                value.Character = Me
            End If
        End Set
    End Property

    Public ReadOnly Property CharacterType As String Implements ICharacter.CharacterType
        Get
            Return CharacterData.Metadatas(MetadataTypes.CharacterType)
        End Get
    End Property

    Public Property MaximumOxygen As Integer Implements ICharacter.MaximumOxygen
        Get
            Return Math.Max(0, CharacterData.Statistics(StatisticTypes.MaximumOxygen))
        End Get
        Set(value As Integer)
            CharacterData.Statistics(StatisticTypes.MaximumOxygen) = Math.Max(0, value)
        End Set
    End Property

    Public Property Oxygen As Integer Implements ICharacter.Oxygen
        Get
            Return Math.Clamp(CharacterData.Statistics(StatisticTypes.Oxygen), 0, MaximumOxygen)
        End Get
        Set(value As Integer)
            CharacterData.Statistics(StatisticTypes.Oxygen) = Math.Clamp(value, 0, MaximumOxygen)
        End Set
    End Property

    Public Property Facing As Integer Implements ICharacter.Facing
        Get
            Return CharacterData.Statistics(StatisticTypes.Facing)
        End Get
        Set(value As Integer)
            CharacterData.Statistics(StatisticTypes.Facing) = value
        End Set
    End Property
End Class
