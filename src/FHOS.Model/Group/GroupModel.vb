Imports FHOS.Persistence

Friend Class GroupModel
    Implements IGroupModel

    Private group As Persistence.IGroup

    Protected Sub New(group As Persistence.IGroup)
        Me.group = group
    End Sub

    Friend Shared Function FromGroup(group As IGroup) As IGroupModel
        If group Is Nothing Then
            Return Nothing
        End If
        Return New GroupModel(group)
    End Function

    Public ReadOnly Property Name As String Implements IGroupModel.Name
        Get
            Return group.Name
        End Get
    End Property

    Private Shared Function ToLevelName(value As Integer) As String
        Select Case value
            Case Is > 90
                Return "Acceptable"
            Case Is > 75
                Return "Tolerable"
            Case Is > 50
                Return "Unacceptable"
            Case Else
                Return "Inexcusable"
        End Select
    End Function

    Public ReadOnly Property Authority As (LevelName As String, Value As Integer) Implements IGroupModel.Authority
        Get
            Return (ToLevelName(group.Authority), group.Authority)
        End Get
    End Property

    Public ReadOnly Property Standards As (LevelName As String, Value As Integer) Implements IGroupModel.Standards
        Get
            Return (ToLevelName(group.Standards), group.Standards)
        End Get
    End Property

    Public ReadOnly Property Conviction As (LevelName As String, Value As Integer) Implements IGroupModel.Conviction
        Get
            Return (ToLevelName(group.Conviction), group.Conviction)
        End Get
    End Property

    Public ReadOnly Property PlanetCount As Integer Implements IGroupModel.PlanetCount
        Get
            Return group.PlanetCount
        End Get
    End Property

    Public ReadOnly Property Actors As IEnumerable(Of IActorModel) Implements IGroupModel.Actors
        Get
            Return group.Universe.Actors.Where(Function(x) x.Properties.Group IsNot Nothing AndAlso x.Properties.Group.Id = group.Id).Select(Function(x) ActorModel.FromActor(x))
        End Get
    End Property

    Public Function RelationNameTo(otherGroup As IGroupModel) As String Implements IGroupModel.RelationNameTo
        Dim deltaAuthority = otherGroup.Authority.Value - Authority.Value
        Dim deltaStandards = otherGroup.Standards.Value - Standards.Value
        Dim deltaConviction = otherGroup.Conviction.Value - Conviction.Value
        Select Case CInt(Math.Sqrt(deltaAuthority * deltaAuthority) + (deltaStandards * deltaStandards) + (deltaConviction * deltaConviction))
            Case Is >= 50
                Return "Hostile"
            Case Is >= 25
                Return "Neutral"
            Case Else
                Return "Friendly"
        End Select
    End Function
End Class
