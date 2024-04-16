Imports FHOS.Persistence

Friend Module StarSystemInitializer
    Private Const SystemMapColumns = 31
    Private Const SystemMapRows = 31

    Friend Sub Initialize(starSystem As IStarSystem, starCell As ICell, starFlag As String)
        starSystem.Map = starSystem.Universe.CreateMap(
            MapTypes.System,
            $"{starSystem.Name} System",
            SystemMapColumns,
            SystemMapRows,
            TerrainTypes.Void)
        Dim teleporter = starSystem.Universe.CreateTeleporter(starCell)
        With starSystem.Map.GetCell(0, 0)
            .TerrainType = VoidNorthWestArrow
            .Teleporter = teleporter
            .SetFlag(starFlag)
        End With
        With starSystem.Map.GetCell(SystemMapColumns - 1, 0)
            .TerrainType = VoidNorthEastArrow
            .Teleporter = teleporter
            .SetFlag(starFlag)
        End With
        With starSystem.Map.GetCell(0, SystemMapRows - 1)
            .TerrainType = VoidSouthWestArrow
            .Teleporter = teleporter
            .SetFlag(starFlag)
        End With
        With starSystem.Map.GetCell(0, SystemMapRows - 1)
            .TerrainType = VoidSouthWestArrow
            .Teleporter = teleporter
            .SetFlag(starFlag)
        End With
        With starSystem.Map.GetCell(SystemMapColumns - 1, SystemMapRows - 1)
            .TerrainType = VoidSouthEastArrow
            .Teleporter = teleporter
            .SetFlag(starFlag)
        End With
        For Each row In Enumerable.Range(1, SystemMapRows - 2)
            With starSystem.Map.GetCell(0, row)
                .Teleporter = teleporter
                .TerrainType = VoidWestArrow
            End With
            With starSystem.Map.GetCell(1, row)
                .SetFlag(starFlag)
            End With
            With starSystem.Map.GetCell(SystemMapColumns - 1, row)
                .Teleporter = teleporter
                .TerrainType = VoidEastArrow
            End With
            With starSystem.Map.GetCell(SystemMapColumns - 2, row)
                .SetFlag(starFlag)
            End With
        Next
        For Each column In Enumerable.Range(1, SystemMapColumns - 2)
            With starSystem.Map.GetCell(column, 0)
                .Teleporter = teleporter
                .TerrainType = VoidNorthArrow
            End With
            With starSystem.Map.GetCell(column, 1)
                .SetFlag(starFlag)
            End With
            With starSystem.Map.GetCell(column, SystemMapRows - 1)
                .Teleporter = teleporter
                .TerrainType = VoidSouthArrow
            End With
            With starSystem.Map.GetCell(column, SystemMapRows - 2)
                .SetFlag(starFlag)
            End With
        Next
    End Sub
End Module
