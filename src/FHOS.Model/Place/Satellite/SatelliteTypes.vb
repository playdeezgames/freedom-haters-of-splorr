Public Module SatelliteTypes
    Friend Const Radiated = "Radiated"
    Friend Const Volcanic = "Volcanic"
    Friend Const Barren = "Barren"
    Friend Const Inferno = "Inferno"
    Friend Const Cavernous = "Cavernous"
    Friend Const Ice = "Ice"

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, SatelliteTypeDescriptor) =
        New Dictionary(Of String, SatelliteTypeDescriptor) From
        {
            {
                Radiated,
                New SatelliteTypeDescriptor(Radiated)
            },
            {
                Volcanic,
                New SatelliteTypeDescriptor(Volcanic)
            },
            {
                Barren,
                New SatelliteTypeDescriptor(Barren)
            },
            {
                Inferno,
                New SatelliteTypeDescriptor(Inferno)
            },
            {
                Cavernous,
                New SatelliteTypeDescriptor(Cavernous)
            },
            {
                Ice,
                New SatelliteTypeDescriptor(Ice)
            }
        }
End Module
