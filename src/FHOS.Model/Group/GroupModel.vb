Imports FHOS.Persistence

Friend Class GroupModel
    Implements IGroupModel

    Private Const HostileText As String = "Hostile"
    Private Const NeutralText As String = "Neutral"
    Private Const FriendlyText As String = "Friendly"
    Private Const HostileThreshold As Integer = 50
    Private Const NeutralThreshold As Integer = 25
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
        Dim deltaAuthority = otherGroup.Properties.Authority.Value - Properties.Authority.Value
        Dim deltaStandards = otherGroup.Properties.Standards.Value - Properties.Standards.Value
        Dim deltaConviction = otherGroup.Properties.Conviction.Value - Properties.Conviction.Value
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
