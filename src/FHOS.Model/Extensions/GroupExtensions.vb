Imports System.Runtime.CompilerServices
Imports FHOS.Persistence

Friend Module GroupExtensions
    <Extension>
    Friend Function SingleParent(group As IGroup, parentGroupType As String) As IGroup
        Return group.Parents.Single(Function(x) x.EntityType = parentGroupType)
    End Function
    <Extension>
    Friend Function ChildrenOfType(group As IGroup, childGroupType As String) As IEnumerable(Of IGroup)
        Return group.Children.Where(Function(x) x.EntityType = childGroupType)
    End Function
End Module
