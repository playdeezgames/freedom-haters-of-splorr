Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class AvatarInitializationStep
    Inherits InitializationStep
    ReadOnly universe As IUniverse
    ReadOnly embarkSettings As EmbarkSettings
    Sub New(universe As IUniverse, embarkSettings As EmbarkSettings)
        Me.universe = universe
        Me.embarkSettings = embarkSettings
    End Sub
    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim actor = universe.Avatar.AvatarActor
        actor.Jools = StartingWealthLevels.Descriptors(embarkSettings.StartingWealthLevel).GenerateJools
        actor.MinimumJools = StartingWealthLevels.Descriptors(embarkSettings.StartingWealthLevel).MinimumJools
    End Sub
End Class
