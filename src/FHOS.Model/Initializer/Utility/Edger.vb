Imports FHOS.Persistence

Friend Module Edger
    Friend Function GetCorners(
                              columns As Integer,
                              rows As Integer) As IReadOnlyList(Of (X As Integer, Y As Integer, LocationType As String))
        Return New List(Of (X As Integer, Y As Integer, LocationType As String)) From
        {
            (0, 0, MakeVoidArrow(NorthWest)),
            (columns - 1, 0, MakeVoidArrow(NorthEast)),
            (0, rows - 1, MakeVoidArrow(SouthWest)),
            (columns - 1, rows - 1, MakeVoidArrow(SouthEast))
        }
    End Function
    Friend Function GetCornerActors(
                              columns As Integer,
                              rows As Integer) As IReadOnlyList(Of (X As Integer, Y As Integer, ActorType As String))
        Return New List(Of (X As Integer, Y As Integer, ActorType As String)) From
        {
            (0, 0, ActorTypes.MakeArrow(NorthWest)),
            (columns - 1, 0, ActorTypes.MakeArrow(NorthEast)),
            (0, rows - 1, ActorTypes.MakeArrow(SouthWest)),
            (columns - 1, rows - 1, ActorTypes.MakeArrow(SouthEast))
        }
    End Function
    Friend Function GetEdges(
                              columns As Integer,
                              rows As Integer) As IReadOnlyList(Of (X As Integer, Y As Integer, LocationType As String, DeltaX As Integer, DeltaY As Integer))
        Return {
            Enumerable.Range(1, columns - 2).Select(Function(x) (x, 0, MakeVoidArrow(OrdinalDirections.North), 0, 1)),
            Enumerable.Range(1, rows - 2).Select(Function(x) (columns - 1, x, MakeVoidArrow(OrdinalDirections.East), -1, 0)),
            Enumerable.Range(1, columns - 2).Select(Function(x) (x, rows - 1, MakeVoidArrow(OrdinalDirections.South), 0, -1)),
            Enumerable.Range(1, rows - 2).Select(Function(x) (0, x, MakeVoidArrow(OrdinalDirections.West), 1, 0))
        }.Aggregate(
            New List(Of (X As Integer, Y As Integer, LocationType As String, DeltaX As Integer, DeltaY As Integer)),
            Function(a, x)
                Dim l = a.ToList
                l.AddRange(x)
                Return l
            End Function).ToList

    End Function
    Friend Function GetEdgeActors(
                              columns As Integer,
                              rows As Integer) As IReadOnlyList(Of (X As Integer, Y As Integer, ActorType As String, DeltaX As Integer, DeltaY As Integer))
        Return {
            Enumerable.Range(1, columns - 2).Select(Function(x) (x, 0, ActorTypes.MakeArrow(OrdinalDirections.North), 0, 1)),
            Enumerable.Range(1, rows - 2).Select(Function(x) (columns - 1, x, ActorTypes.MakeArrow(OrdinalDirections.East), -1, 0)),
            Enumerable.Range(1, columns - 2).Select(Function(x) (x, rows - 1, ActorTypes.MakeArrow(OrdinalDirections.South), 0, -1)),
            Enumerable.Range(1, rows - 2).Select(Function(x) (0, x, ActorTypes.MakeArrow(OrdinalDirections.West), 1, 0))
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
            .EntityType = locationType
            .TargetLocation = targetLocation
        End With
    End Sub
    Friend Sub PlaceBoundaryActor(location As ILocation, actorType As String, targetActor As IActor)
        Dim actor = ActorTypes.Descriptors(actorType).CreateActor(location, "Leave Area")
        actor.YokedActor(YokeTypes.Target) = targetActor
    End Sub
    Friend Sub PlaceBoundaryActors(targetActor As IActor, columns As Integer, rows As Integer)
        Dim map = targetActor.Properties.Interior
        For Each corner In GetCornerActors(columns, rows)
            PlaceBoundaryActor(map.GetLocation(corner.X, corner.Y), corner.ActorType, targetActor)
        Next
        For Each edge In GetEdgeActors(columns, rows)
            PlaceBoundaryActor(map.GetLocation(edge.X, edge.Y), edge.ActorType, targetActor)
            map.GetLocation(edge.X + edge.DeltaX, edge.Y + edge.DeltaY).Flags(FlagTypes.IsEdge) = True
        Next
    End Sub
End Module
