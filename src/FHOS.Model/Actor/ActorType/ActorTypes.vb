Friend Module ActorTypes
    Friend Const Player = "Player"
    Friend Const Enemy = "Enemy"
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, ActorTypeDescriptor) =
        New Dictionary(Of String, ActorTypeDescriptor) From
        {
            {
                Player,
                New ActorTypeDescriptor(
                    Player,
                    {ChrW(128), ChrW(129), ChrW(130), ChrW(131)},
                    Hue.LightGray,
                    maximumOxygen:=100,
                    maximumFuel:=100,
                    canSpawn:=Function(x) x.LocationType = LocationTypes.Void AndAlso x.Actor Is Nothing)
            },
            {
                Enemy,
                New ActorTypeDescriptor(
                    Enemy,
                    {ChrW(132), ChrW(133), ChrW(134), ChrW(135)},
                    Hue.DarkGray,
                    maximumOxygen:=100,
                    maximumFuel:=100,
                    spawnCount:=25,
                    canSpawn:=Function(x) x.LocationType = LocationTypes.Void AndAlso x.Actor Is Nothing)
            }
        }
End Module
