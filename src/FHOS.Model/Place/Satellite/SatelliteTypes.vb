Public Module SatelliteTypes
    Friend Const Radiated = "Radiated"
    Friend Const Volcanic = "Volcanic"
    Friend Const Barren = "Barren"
    Friend Const Inferno = "Inferno"
    Friend Const Cavernous = "Cavernous"
    Friend Const Ice = "Ice"

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, SatelliteDescriptor) =
        New Dictionary(Of String, SatelliteDescriptor) From
        {
            {
                Radiated,
                New SatelliteDescriptor(LocationTypes.RadiatedMoon)
            },
            {
                Volcanic,
                New SatelliteDescriptor(LocationTypes.VolcanicMoon)
            },
            {
                Barren,
                New SatelliteDescriptor(LocationTypes.BarrenMoon)
            },
            {
                Inferno,
                New SatelliteDescriptor(LocationTypes.InfernoMoon)
            },
            {
                Cavernous,
                New SatelliteDescriptor(LocationTypes.CavernousMoon)
            },
            {
                Ice,
                New SatelliteDescriptor(LocationTypes.IceMoon)
            }
        }
End Module
