Imports FHOS.Persistence
Imports SPLORR.Game

Friend Module AvatarInitializer
    Friend Function Initialize(map As IMap, startingWealthLevel As String) As IActor
        Dim actorLocation As ILocation
        Dim descriptor = ActorTypes.Descriptors(Player)
        Do
            actorLocation = map.GetLocation(RNG.FromRange(0, map.Size.Columns - 1), RNG.FromRange(0, map.Size.Rows - 1))
        Loop Until descriptor.CanSpawn(actorLocation)
        Dim actor = actorLocation.CreateActor(Player)
        actor.MaximumOxygen = descriptor.MaximumOxygen
        actor.Oxygen = descriptor.MaximumOxygen
        actor.MaximumFuel = descriptor.MaximumFuel
        actor.Fuel = descriptor.MaximumFuel
        actor.Jools = StartingWealthLevels.Descriptors(startingWealthLevel).GenerateJools
        Return actor
    End Function
End Module
