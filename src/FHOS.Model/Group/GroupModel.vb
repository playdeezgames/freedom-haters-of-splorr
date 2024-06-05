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
            Return group.LegacyEntityName
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
            Return (ToLevelName(group.Statistics(StatisticTypes.Authority).Value), group.Statistics(StatisticTypes.Authority).Value)
        End Get
    End Property

    Public ReadOnly Property Standards As (LevelName As String, Value As Integer) Implements IGroupModel.Standards
        Get
            Return (ToLevelName(group.Statistics(StatisticTypes.Standards).Value), group.Statistics(StatisticTypes.Standards).Value)
        End Get
    End Property

    Public ReadOnly Property Conviction As (LevelName As String, Value As Integer) Implements IGroupModel.Conviction
        Get
            Return (ToLevelName(group.Statistics(StatisticTypes.Conviction).Value), group.Statistics(StatisticTypes.Conviction).Value)
        End Get
    End Property

    Public ReadOnly Property PlanetCount As Integer Implements IGroupModel.PlanetCount
        Get
            Return group.Statistics(StatisticTypes.PlanetCount).Value
        End Get
    End Property

    Public ReadOnly Property PlanetList As IEnumerable(Of IGroupModel) Implements IGroupModel.PlanetList
        Get
            Return group.Children.Where(Function(x) x.EntityType = GroupTypes.PlanetVicinity).Select(AddressOf GroupModel.FromGroup).OrderBy(Function(x) x.Name)
        End Get
    End Property

    Public ReadOnly Property SatelliteList As IEnumerable(Of IGroupModel) Implements IGroupModel.SatelliteList
        Get
            Return group.Children.Where(Function(x) x.EntityType = GroupTypes.Satellite).Select(AddressOf GroupModel.FromGroup).OrderBy(Function(x) x.Name)
        End Get
    End Property

    Public ReadOnly Property Pedia As IUniversePediaModel Implements IGroupModel.Pedia
        Get
            Return UniversePediaModel.FromUniverse(group.Universe)
        End Get
    End Property

    Public ReadOnly Property StarSystem As IGroupModel Implements IGroupModel.StarSystem
        Get
            Return GroupModel.FromGroup(group.Parents.SingleOrDefault(Function(x) x.EntityType = GroupTypes.StarSystem))
        End Get
    End Property

    Public ReadOnly Property Planet As IGroupModel Implements IGroupModel.Planet
        Get
            Return GroupModel.FromGroup(group.Parents.SingleOrDefault(Function(x) x.EntityType = GroupTypes.PlanetVicinity))
        End Get
    End Property

    Public ReadOnly Property Faction As IGroupModel Implements IGroupModel.Faction
        Get
            Return GroupModel.FromGroup(group.Parents.SingleOrDefault(Function(x) x.EntityType = GroupTypes.Faction))
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
