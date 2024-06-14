Imports FHOS.Persistence

Friend Class GroupModel
    Implements IGroupModel

    Private Const HostileText As String = "Hostile"
    Private Const NeutralText As String = "Neutral"
    Private Const FriendlyText As String = "Friendly"
    Private Const HostileThreshold As Integer = 50
    Private Const NeutralThreshold As Integer = 25
    Private Const AcceptableText As String = "Acceptable"
    Private Const TolerableText As String = "Tolerable"
    Private Const UnacceptableText As String = "Unacceptable"
    Private Const AcceptableThreshold = 90
    Private Const TolerableThreshold = 75
    Private Const UnacceptableThreshold = 50
    Private Const InexcusableText As String = "Inexcusable"
    Private ReadOnly group As Persistence.IGroup

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
            Return group.EntityName
        End Get
    End Property

    Private Shared Function ToLevelName(value As Integer) As String
        Select Case value
            Case Is > AcceptableThreshold
                Return AcceptableText
            Case Is > TolerableThreshold
                Return TolerableText
            Case Is > UnacceptableThreshold
                Return UnacceptableText
            Case Else
                Return InexcusableText
        End Select
    End Function

    Private Function ToLevelAndValue(statisticType As String) As (LevelName As String, Value As Integer)
        Dim statisticValue = group.Statistics(statisticType).Value
        Return (ToLevelName(statisticValue), statisticValue)
    End Function

    Public ReadOnly Property Authority As (LevelName As String, Value As Integer) Implements IGroupModel.Authority
        Get
            Return ToLevelAndValue(StatisticTypes.Authority)
        End Get
    End Property

    Public ReadOnly Property Standards As (LevelName As String, Value As Integer) Implements IGroupModel.Standards
        Get
            Return ToLevelAndValue(StatisticTypes.Standards)
        End Get
    End Property

    Public ReadOnly Property Conviction As (LevelName As String, Value As Integer) Implements IGroupModel.Conviction
        Get
            Return ToLevelAndValue(StatisticTypes.Conviction)
        End Get
    End Property

    Public ReadOnly Property PlanetCount As Integer Implements IGroupModel.PlanetCount
        Get
            Return group.Statistics(StatisticTypes.PlanetCount).Value
        End Get
    End Property

    Private Function SortedChildrenOfType(groupType As String) As IEnumerable(Of IGroupModel)
        Return group.ChildrenOfType(groupType).Select(AddressOf GroupModel.FromGroup).OrderBy(Function(x) x.Name)
    End Function

    Public ReadOnly Property ChildPlanets As IEnumerable(Of IGroupModel) Implements IGroupModel.ChildPlanets
        Get
            Return SortedChildrenOfType(GroupTypes.PlanetVicinity)
        End Get
    End Property

    Public ReadOnly Property ChildSatellites As IEnumerable(Of IGroupModel) Implements IGroupModel.ChildSatellites
        Get
            Return SortedChildrenOfType(GroupTypes.Satellite)
        End Get
    End Property

    Private Function OptionalSingleParent(groupType As String) As IGroupModel
        Return GroupModel.FromGroup(group.Parents.SingleOrDefault(Function(x) x.EntityType = groupType))
    End Function

    Public ReadOnly Property ParentStarSystem As IGroupModel Implements IGroupModel.ParentStarSystem
        Get
            Return OptionalSingleParent(GroupTypes.StarSystem)
        End Get
    End Property

    Public ReadOnly Property ParentPlanet As IGroupModel Implements IGroupModel.ParentPlanet
        Get
            Return OptionalSingleParent(GroupTypes.PlanetVicinity)
        End Get
    End Property

    Public ReadOnly Property ParentFaction As IGroupModel Implements IGroupModel.ParentFaction
        Get
            Return OptionalSingleParent(GroupTypes.Faction)
        End Get
    End Property

    Public ReadOnly Property StarTypeName As String Implements IGroupModel.StarTypeName
        Get
            Dim subType = group.Metadatas(MetadataTypes.Subtype)
            If subType IsNot Nothing Then
                Return StarTypes.Descriptors(subType).StarTypeName
            End If
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property SatelliteCount As Integer Implements IGroupModel.SatelliteCount
        Get
            Return group.Statistics(StatisticTypes.SatelliteCount).Value
        End Get
    End Property

    Public ReadOnly Property Position As (Column As Integer, Row As Integer) Implements IGroupModel.Position
        Get
            Return (group.Statistics(StatisticTypes.Column).Value, group.Statistics(StatisticTypes.Row).Value)
        End Get
    End Property

    Public ReadOnly Property ChildPlanetFactions As IEnumerable(Of IGroupModel) Implements IGroupModel.ChildPlanetFactions
        Get
            Dim planetVicinities = group.ChildrenOfType(GroupTypes.PlanetVicinity)
            Return planetVicinities.
                Select(Function(x) x.SingleParent(GroupTypes.Faction)).
                GroupBy(Function(z) z.Id).
                Select(Function(w) w.First).
                Select(AddressOf GroupModel.FromGroup)
        End Get
    End Property

    Public ReadOnly Property Properties As IGroupPropertiesModel Implements IGroupModel.Properties
        Get
            Return GroupPropertiesModel.FromGroup(group)
        End Get
    End Property

    Public Function RelationNameTo(otherGroup As IGroupModel) As String Implements IGroupModel.RelationNameTo
        Dim deltaAuthority = otherGroup.Authority.Value - Authority.Value
        Dim deltaStandards = otherGroup.Standards.Value - Standards.Value
        Dim deltaConviction = otherGroup.Conviction.Value - Conviction.Value
        Select Case CInt(Math.Sqrt(deltaAuthority * deltaAuthority) + (deltaStandards * deltaStandards) + (deltaConviction * deltaConviction))
            Case Is >= HostileThreshold
                Return HostileText
            Case Is >= NeutralThreshold
                Return NeutralText
            Case Else
                Return FriendlyText
        End Select
    End Function
End Class
