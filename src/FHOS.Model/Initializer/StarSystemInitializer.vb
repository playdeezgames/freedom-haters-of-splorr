Imports FHOS.Persistence

Friend Module StarSystemInitializer
    Private Const SystemMapColumns = 31
    Private Const SystemMapRows = 31

    Friend Sub Initialize(starSystem As IStarSystem, starLocation As ILocation, starFlag As String)
        starSystem.Map = starSystem.Universe.CreateMap(
            MapTypes.System,
            $"{starSystem.Name} System",
            SystemMapColumns,
            SystemMapRows,
            LocationTypes.Void)
        PlaceSystemBoundaries(starSystem, starLocation, starFlag)
        PlaceStar(starSystem)
        'PlacePlanets()
    End Sub

    Private Sub PlaceStar(starSystem As IStarSystem)
        Dim starColumn = SystemMapColumns \ 2
        Dim starRow = SystemMapRows \ 2
        Dim locationType = StarTypes.Descriptors(starSystem.StarType).LocationType
        starSystem.Map.GetLocation(starColumn, starRow).LocationType = locationType
    End Sub

    Private Sub PlaceSystemBoundaries(starSystem As IStarSystem, starLocation As ILocation, starFlag As String)
        Dim teleporter = starSystem.Universe.CreateTeleporter(starLocation)
        With starSystem.Map.GetLocation(0, 0)
            .LocationType = VoidNorthWestArrow
            .Teleporter = teleporter
            .SetFlag(starFlag)
        End With
        With starSystem.Map.GetLocation(SystemMapColumns - 1, 0)
            .LocationType = VoidNorthEastArrow
            .Teleporter = teleporter
            .SetFlag(starFlag)
        End With
        With starSystem.Map.GetLocation(0, SystemMapRows - 1)
            .LocationType = VoidSouthWestArrow
            .Teleporter = teleporter
            .SetFlag(starFlag)
        End With
        With starSystem.Map.GetLocation(0, SystemMapRows - 1)
            .LocationType = VoidSouthWestArrow
            .Teleporter = teleporter
            .SetFlag(starFlag)
        End With
        With starSystem.Map.GetLocation(SystemMapColumns - 1, SystemMapRows - 1)
            .LocationType = VoidSouthEastArrow
            .Teleporter = teleporter
            .SetFlag(starFlag)
        End With
        For Each row In Enumerable.Range(1, SystemMapRows - 2)
            With starSystem.Map.GetLocation(0, row)
                .Teleporter = teleporter
                .LocationType = VoidWestArrow
            End With
            With starSystem.Map.GetLocation(1, row)
                .SetFlag(starFlag)
            End With
            With starSystem.Map.GetLocation(SystemMapColumns - 1, row)
                .Teleporter = teleporter
                .LocationType = VoidEastArrow
            End With
            With starSystem.Map.GetLocation(SystemMapColumns - 2, row)
                .SetFlag(starFlag)
            End With
        Next
        For Each column In Enumerable.Range(1, SystemMapColumns - 2)
            With starSystem.Map.GetLocation(column, 0)
                .Teleporter = teleporter
                .LocationType = VoidNorthArrow
            End With
            With starSystem.Map.GetLocation(column, 1)
                .SetFlag(starFlag)
            End With
            With starSystem.Map.GetLocation(column, SystemMapRows - 1)
                .Teleporter = teleporter
                .LocationType = VoidSouthArrow
            End With
            With starSystem.Map.GetLocation(column, SystemMapRows - 2)
                .SetFlag(starFlag)
            End With
        Next
    End Sub
End Module
