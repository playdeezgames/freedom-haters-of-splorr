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
                New SatelliteDescriptor(Radiated)
            },
            {
                Volcanic,
                New SatelliteDescriptor(Volcanic)
            },
            {
                Barren,
                New SatelliteDescriptor(Barren)
            },
            {
                Inferno,
                New SatelliteDescriptor(Inferno)
            },
            {
                Cavernous,
                New SatelliteDescriptor(Cavernous)
            },
            {
                Ice,
                New SatelliteDescriptor(Ice)
            }
        }
End Module
