Imports System.Runtime.CompilerServices
Imports FHOS.Persistence

Friend Module LocationExtensions
    <Extension>
    Friend Function GetCardinalNeighbor(location As ILocation, direction As String) As ILocation
        Dim delta = CardinalDirections.Descriptors(direction).Delta
        Dim nextColumn = location.Position.Column + delta.X
        Dim nextRow = location.Position.Row + delta.Y
        Return location.Map.GetLocation(nextColumn, nextRow)
    End Function
    <Extension>
    Friend Function GetOrdinalNeighbor(location As ILocation, direction As String) As ILocation
        Dim delta = OrdinalDirections.Descriptors(direction).Delta
        Dim nextColumn = location.Position.Column + delta.X
        Dim nextRow = location.Position.Row + delta.Y
        Return location.Map.GetLocation(nextColumn, nextRow)
    End Function
    <Extension>
    Friend Function GetCardinalNeighbors(location As ILocation) As IEnumerable(Of ILocation)
        Dim result As New List(Of ILocation)
        For Each value In CardinalDirections.Descriptors.Keys
            Dim neighbor = location.GetCardinalNeighbor(value)
            If neighbor IsNot Nothing Then
                result.Add(neighbor)
            End If
        Next
        Return result
    End Function
    <Extension>
    Friend Function GetOrdinalNeighbors(location As ILocation) As IEnumerable(Of ILocation)
        Dim result As New List(Of ILocation)
        For Each value In OrdinalDirections.Descriptors.Keys
            Dim neighbor = location.GetOrdinalNeighbor(value)
            If neighbor IsNot Nothing Then
                result.Add(neighbor)
            End If
        Next
        Return result
    End Function
    <Extension>
    Friend Function GetEmptyOrdinalNeighborsOfType(location As ILocation, locationType As String) As IEnumerable(Of ILocation)
        Return GetEmptyOrdinalNeighbors(location).Where(Function(x) x.EntityType = locationType)
    End Function
    <Extension>
    Friend Function GetEmptyCardinalNeighborsOfType(location As ILocation, locationType As String) As IEnumerable(Of ILocation)
        Return GetEmptyCardinalNeighbors(location).Where(Function(x) x.EntityType = locationType)
    End Function
    <Extension>
    Friend Function GetEmptyCardinalNeighbors(location As ILocation) As IEnumerable(Of ILocation)
        Return GetCardinalNeighbors(location).Where(Function(x) x.Actor Is Nothing)
    End Function
    <Extension>
    Friend Function GetEmptyOrdinalNeighbors(location As ILocation) As IEnumerable(Of ILocation)
        Return GetOrdinalNeighbors(location).Where(Function(x) x.Actor Is Nothing)
    End Function
End Module
