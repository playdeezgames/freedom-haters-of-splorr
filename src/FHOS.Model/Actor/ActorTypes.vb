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
                    100,
                    Function(x) x.LocationType = LocationTypes.Void)}
        }
End Module
