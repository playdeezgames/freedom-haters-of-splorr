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
    Public ReadOnly Property Properties As IGroupPropertiesModel Implements IGroupModel.Properties
        Get
            Return GroupPropertiesModel.FromGroup(group)
        End Get
    End Property

    Public ReadOnly Property Parents As IGroupParentsModel Implements IGroupModel.Parents
        Get
            Return GroupParentsModel.FromGroup(group)
        End Get
    End Property

    Public ReadOnly Property Children As IGroupChildrenModel Implements IGroupModel.Children
        Get
            Return GroupChildrenModel.FromGroup(group)
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
    Friend Function ToGroup() As IGroup
        Return group
    End Function
End Class
