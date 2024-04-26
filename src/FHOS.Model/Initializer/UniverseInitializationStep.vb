Imports FHOS.Persistence

Friend Class UniverseInitializationStep
    Inherits InitializationStep
    Private ReadOnly universe As IUniverse
    Private ReadOnly embarkSettings As EmbarkSettings
    Sub New(universe As IUniverse, embarkSettings As EmbarkSettings)
        Me.universe = universe
        Me.embarkSettings = embarkSettings
    End Sub
    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep))
        GalaxyInitializer.Initialize(universe, embarkSettings)
    End Sub
End Class
