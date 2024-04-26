Imports FHOS.Persistence

Friend Module UniverseInitializer

    Sub Initialize(universe As IUniverse, embarkSettings As EmbarkSettings)
        Dim galaxy = GalaxyInitializer.Initialize(universe, embarkSettings.GalacticAge, embarkSettings.GalacticDensity)
        universe.Avatar = AvatarInitializer.Initialize(galaxy, embarkSettings.StartingWealthLevel)
    End Sub
End Module
