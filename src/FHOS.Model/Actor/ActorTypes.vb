Friend Module ActorTypes
    Friend Const Player = "Player"
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, ActorTypeDescriptor) =
        New Dictionary(Of String, ActorTypeDescriptor) From
        {
            {
                Player,
                New ActorTypeDescriptor(
                    {ChrW(128), ChrW(129), ChrW(130), ChrW(131)},
                    Hue.LightGray,
                    maximumOxygen:=100,
                    maximumFuel:=100,
                    canSpawn:=Function(x) x.LocationType = LocationTypes.Void)}
        }
End Module
