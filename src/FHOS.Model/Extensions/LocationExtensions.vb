Imports System.Runtime.CompilerServices
Imports FHOS.Persistence

Friend Module LocationExtensions
    <Extension>
    Friend Function GetNeighbor(location As ILocation, facing As Integer) As ILocation
        Dim delta = Facings.Deltas(facing)
        Dim nextColumn = location.Position.Column + delta.X
        Dim nextRow = location.Position.Row + delta.Y
        Return location.Map.GetLocation(nextColumn, nextRow)
    End Function
    <Extension>
    Friend Function GetNeighbors(location As ILocation) As IEnumerable(Of ILocation)
        Dim result As New List(Of ILocation)
        For Each value In Facings.All
            Dim neighbor = location.GetNeighbor(value)
            If neighbor IsNot Nothing Then
                result.Add(neighbor)
            End If
        Next
        Return result
    End Function
    <Extension>
    Friend Function GetEmptyNeighborsOfType(location As ILocation, locationType As String) As IEnumerable(Of ILocation)
        Return GetEmptyNeighbors(location).Where(Function(x) x.EntityType = locationType)
    End Function
    <Extension>
    Friend Function GetEmptyNeighbors(location As ILocation) As IEnumerable(Of ILocation)
        Return GetNeighbors(location).Where(Function(x) x.Actor Is Nothing)
    End Function
End Module
