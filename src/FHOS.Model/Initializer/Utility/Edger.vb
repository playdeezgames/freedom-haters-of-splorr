Imports FHOS.Persistence

Friend Module Edger
    Friend Function GetCorners(
                              columns As Integer,
                              rows As Integer) As IReadOnlyList(Of (X As Integer, Y As Integer, LocationType As String))
        Return New List(Of (X As Integer, Y As Integer, LocationType As String)) From
        {
            (0, 0, VoidNorthWestArrow),
            (columns - 1, 0, VoidNorthEastArrow),
            (0, rows - 1, VoidSouthWestArrow),
            (columns - 1, rows - 1, VoidSouthEastArrow)
        }
    End Function
    Friend Function GetEdges(
                              columns As Integer,
                              rows As Integer) As IReadOnlyList(Of (X As Integer, Y As Integer, LocationType As String, DeltaX As Integer, DeltaY As Integer))
        Return {
            Enumerable.Range(1, columns - 2).Select(Function(x) (x, 0, VoidNorthArrow, 0, 1)),
            Enumerable.Range(1, rows - 2).Select(Function(x) (columns - 1, x, VoidEastArrow, -1, 0)),
            Enumerable.Range(1, columns - 2).Select(Function(x) (x, rows - 1, VoidSouthArrow, 0, -1)),
            Enumerable.Range(1, rows - 2).Select(Function(x) (0, x, VoidWestArrow, 1, 0))
        }.Aggregate(
            New List(Of (X As Integer, Y As Integer, LocationType As String, DeltaX As Integer, DeltaY As Integer)),
            Function(a, x)
                Dim l = a.ToList
                l.AddRange(x)
                Return l
            End Function).ToList

    End Function
    Friend Sub PlaceBoundary(location As ILocation, locationType As String, targetLocation As ILocation)
        With location
            .LocationType = locationType
            .TargetLocation = targetLocation
        End With
    End Sub
    Friend Sub PlaceBoundaries(place As IPlace, targetLocation As ILocation, columns As Integer, rows As Integer)
        Dim identifier = place.Identifier
        For Each corner In GetCorners(columns, rows)
            PlaceBoundary(place.Map.GetLocation(corner.X, corner.Y), corner.LocationType, targetLocation)
        Next
        For Each edge In GetEdges(columns, rows)
            PlaceBoundary(place.Map.GetLocation(edge.X, edge.Y), edge.LocationType, targetLocation)
            place.Map.GetLocation(edge.X + edge.DeltaX, edge.Y + edge.DeltaY).SetFlag(identifier)
        Next
    End Sub
End Module
