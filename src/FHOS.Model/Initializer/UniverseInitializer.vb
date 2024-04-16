Imports System.Net.Mail
Imports FHOS.Persistence
Imports SPLORR.Game

Friend Module UniverseInitializer

    Sub Initialize(universe As IUniverse, galacticAge As String, galacticDensity As String)
        Dim galaxy = GalaxyInitializer.Initialize(universe, galacticAge, galacticDensity)
        universe.Avatar = AvatarInitializer.Initialize(galaxy)
    End Sub
End Module
