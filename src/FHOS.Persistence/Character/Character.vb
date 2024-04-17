Friend Class Character
    Inherits ActorDataClient
    Implements ICharacter

    Public Sub New(worldData As Data.UniverseData, characterId As Integer)
        MyBase.New(worldData, characterId)
    End Sub

    Public ReadOnly Property Id As Integer Implements ICharacter.Id
        Get
            Return ActorId
        End Get
    End Property

    Public Property Cell As ICell Implements ICharacter.Cell
        Get
            Return New Cell(UniverseData, ActorData.Statistics(StatisticTypes.CellId))
        End Get
        Set(value As ICell)
            If value.Id <> ActorData.Statistics(StatisticTypes.CellId) Then
                Cell.Character = Nothing
                ActorData.Statistics(StatisticTypes.CellId) = value.Id
                value.Character = Me
                TriggerTutorial(value.Tutorial)
            End If
        End Set
    End Property

    Private Sub TriggerTutorial(tutorial As String)
        If tutorial Is Nothing Then
            Return
        End If
        ActorData.Tutorials.Enqueue(tutorial)
    End Sub

    Public Sub DismissTutorial() Implements ICharacter.DismissTutorial
        If HasTutorial Then
            ActorData.Tutorials.Dequeue()
        End If
    End Sub

    Public Function HasFlag(flag As String) As Boolean Implements ICharacter.HasFlag
        Return ActorData.Flags.Contains(flag)
    End Function

    Public Sub SetFlag(flag As String) Implements ICharacter.SetFlag
        ActorData.Flags.Add(flag)
    End Sub

    Public ReadOnly Property CharacterType As String Implements ICharacter.CharacterType
        Get
            Return ActorData.Metadatas(MetadataTypes.CharacterType)
        End Get
    End Property

    Public Property MaximumOxygen As Integer Implements ICharacter.MaximumOxygen
        Get
            Return Math.Max(0, ActorData.Statistics(StatisticTypes.MaximumOxygen))
        End Get
        Set(value As Integer)
            ActorData.Statistics(StatisticTypes.MaximumOxygen) = Math.Max(0, value)
        End Set
    End Property

    Public Property Oxygen As Integer Implements ICharacter.Oxygen
        Get
            Return Math.Clamp(ActorData.Statistics(StatisticTypes.Oxygen), 0, MaximumOxygen)
        End Get
        Set(value As Integer)
            ActorData.Statistics(StatisticTypes.Oxygen) = Math.Clamp(value, 0, MaximumOxygen)
        End Set
    End Property

    Public Property Facing As Integer Implements ICharacter.Facing
        Get
            Return ActorData.Statistics(StatisticTypes.Facing)
        End Get
        Set(value As Integer)
            ActorData.Statistics(StatisticTypes.Facing) = value
        End Set
    End Property

    Public ReadOnly Property HasTutorial As Boolean Implements ICharacter.HasTutorial
        Get
            Return ActorData.Tutorials.Any
        End Get
    End Property

    Public ReadOnly Property CurrentTutorial As String Implements ICharacter.CurrentTutorial
        Get
            If HasTutorial Then
                Return ActorData.Tutorials.Peek
            End If
            Return Nothing
        End Get
    End Property
End Class
