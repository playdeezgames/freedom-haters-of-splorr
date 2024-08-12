Imports System.Runtime.CompilerServices
Imports FHOS.Persistence
Imports SPLORR.Game

Friend Module GroupExtensions
    <Extension>
    Friend Function SingleParent(group As IGroup, parentGroupType As String) As IGroup
        Return group.Parents.Single(Function(x) x.EntityType = parentGroupType)
    End Function
    <Extension>
    Friend Function ChildrenOfType(group As IGroup, childGroupType As String) As IEnumerable(Of IGroup)
        Return group.Children.Where(Function(x) x.EntityType = childGroupType)
    End Function
    <Extension>
    Friend Sub GenerateValues(group As IGroup)
        Const ValueAttempts = 3
        For Each attempt In Enumerable.Range(1, ValueAttempts)
            group.AddValue(RNG.FromRange(0, GroupValues.Descriptors.Count - 1))
        Next
    End Sub
End Module
