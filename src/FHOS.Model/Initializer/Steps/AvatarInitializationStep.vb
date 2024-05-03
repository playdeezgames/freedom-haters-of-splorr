Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class AvatarInitializationStep
    Inherits InitializationStep
    ReadOnly universe As IUniverse
    ReadOnly starMap As IMap
    ReadOnly embarkSettings As EmbarkSettings
    Sub New(universe As IUniverse, starMap As IMap, embarkSettings As EmbarkSettings)
        Me.universe = universe
        Me.starMap = starMap
        Me.embarkSettings = embarkSettings
    End Sub
    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim actorLocation As ILocation
        Dim descriptor = ActorTypes.Descriptors(Player)
        Do
            actorLocation = starMap.GetLocation(RNG.FromRange(0, starMap.Size.Columns - 1), RNG.FromRange(0, starMap.Size.Rows - 1))
        Loop Until descriptor.CanSpawn(actorLocation)
        Dim actor = actorLocation.CreateActor(Player)
        actor.MaximumOxygen = descriptor.MaximumOxygen
        actor.Oxygen = descriptor.MaximumOxygen
        actor.MaximumFuel = descriptor.MaximumFuel
        actor.Fuel = descriptor.MaximumFuel
        actor.Jools = StartingWealthLevels.Descriptors(embarkSettings.StartingWealthLevel).GenerateJools
        actor.MinimumJools = StartingWealthLevels.Descriptors(embarkSettings.StartingWealthLevel).MinimumJools
        universe.Avatar = actor
    End Sub
End Class
