Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class AvatarModel
    Implements IAvatarModel

    Private ReadOnly avatar As ICharacter

    Public Sub New(avatar As ICharacter)
        Me.avatar = avatar
    End Sub

    Public ReadOnly Property X As Integer Implements IAvatarModel.X
        Get
            Return avatar.Cell.Column
        End Get
    End Property

    Public ReadOnly Property Y As Integer Implements IAvatarModel.Y
        Get
            Return avatar.Cell.Row
        End Get
    End Property

    Public ReadOnly Property MapName As String Implements IAvatarModel.MapName
        Get
            Return avatar.Cell.Map.Name
        End Get
    End Property

    Public ReadOnly Property OxygenHue As Integer Implements IAvatarModel.OxygenHue
        Get
            If OxygenPercent <= 33 Then
                Return Hue.Red
            End If
            If OxygenPercent <= 66 Then
                Return Hue.Yellow
            End If
            Return Hue.Green
        End Get
    End Property

    Public ReadOnly Property OxygenPercent As Integer Implements IAvatarModel.OxygenPercent
        Get
            Return avatar.Oxygen * 100 \ avatar.MaximumOxygen
        End Get
    End Property

    Public ReadOnly Property HasActions As Boolean Implements IAvatarModel.HasActions
        Get
            Return avatar.Cell.StarSystem IsNot Nothing
        End Get
    End Property

    Public ReadOnly Property StarSystem As IStarSystemModel Implements IAvatarModel.StarSystem
        Get
            If avatar.Cell.StarSystem Is Nothing Then
                Return Nothing
            End If
            Return New StarSystemModel(avatar.Cell.StarSystem)
        End Get
    End Property

    Public ReadOnly Property HasTutorial As Boolean Implements IAvatarModel.HasTutorial
        Get
            Return avatar.HasTutorial
        End Get
    End Property

    Public ReadOnly Property CurrentTutorial As String Implements IAvatarModel.CurrentTutorial
        Get
            Return avatar.CurrentTutorial
        End Get
    End Property

    Public ReadOnly Property IgnoreCurrentTutorial As Boolean Implements IAvatarModel.IgnoreCurrentTutorial
        Get
            If Not HasTutorial Then
                Return True
            End If
            Dim descriptor = TutorialTypes.Descriptors(CurrentTutorial)
            If descriptor.HasIgnoreFlag Then
                Return avatar.HasFlag(descriptor.IgnoreFlag)
            End If
            Return False
        End Get
    End Property

    Public ReadOnly Property CanEnterStarSystem As Boolean Implements IAvatarModel.CanEnterStarSystem
        Get
            Return StarSystem IsNot Nothing
        End Get
    End Property

    Public Sub Move(delta As (X As Integer, Y As Integer)) Implements IAvatarModel.Move
        Dim cell = avatar.Cell
        Dim nextColumn = cell.Column + delta.X
        Dim nextRow = cell.Row + delta.Y
        Dim map = cell.Map
        Dim nextCell = map.GetCell(nextColumn, nextRow)
        If nextCell IsNot Nothing Then
            avatar.Cell = nextCell
        End If
    End Sub

    Public Sub SetFacing(facing As Integer) Implements IAvatarModel.SetFacing
        avatar.Facing = facing
    End Sub

    Public Sub DismissTutorial() Implements IAvatarModel.DismissTutorial
        Dim descriptor = TutorialTypes.Descriptors(avatar.CurrentTutorial)
        If descriptor.HasIgnoreFlag Then
            avatar.SetFlag(descriptor.IgnoreFlag)
        End If
        avatar.DismissTutorial()
    End Sub

    Public Sub EnterStarSystem() Implements IAvatarModel.EnterStarSystem
        If CanEnterStarSystem Then
            With avatar.Cell.StarSystem
                avatar.Cell = RNG.FromEnumerable(.Map.Cells.Where(Function(x) x.HasFlag(.Name)))
            End With
        End If
    End Sub
End Class
