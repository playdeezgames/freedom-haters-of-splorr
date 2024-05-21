Imports System.Runtime.CompilerServices
Imports FHOS.Persistence

Friend Module LocationExtensions
    <Extension>
    Friend Function GetNeighbor(location As ILocation, facing As Integer) As ILocation
        Dim delta = Persistence.Facing.Deltas(facing)
        Dim nextColumn = location.Column + delta.X
        Dim nextRow = location.Row + delta.Y
        Return location.Map.GetLocation(nextColumn, nextRow)
    End Function
    <Extension>
    Friend Function GetNeighbors(location As ILocation) As IEnumerable(Of ILocation)
        Dim result As New List(Of ILocation)
        For Each facing In Persistence.Facing.All
            Dim neighbor = location.GetNeighbor(facing)
            If neighbor IsNot Nothing Then
                result.Add(neighbor)
            End If
        Next
        Return result
    End Function
End Module
