Imports FHOS.Persistence

Friend Class GroupChildrenModel
    Implements IGroupChildrenModel

    Private ReadOnly group As IGroup

    Public Sub New(group As IGroup)
        Me.group = group
    End Sub

    Friend Shared Function FromGroup(group As IGroup) As IGroupChildrenModel
        Return New GroupChildrenModel(group)
    End Function
End Class
