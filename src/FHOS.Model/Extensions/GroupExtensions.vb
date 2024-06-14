Imports System.Runtime.CompilerServices
Imports FHOS.Persistence

Friend Module GroupExtensions
    <Extension>
    Friend Function SingleParent(group As IGroup, parentGroupType As String) As IGroup
        Return group.Parents.Single(Function(x) x.EntityType = parentGroupType)
    End Function
End Module
