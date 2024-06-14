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
                New SatelliteTypeDescriptor(Radiated, Hues.Yellow)
            },
            {
                Volcanic,
                New SatelliteTypeDescriptor(Volcanic, Hues.Orange)
            },
            {
                Barren,
                New SatelliteTypeDescriptor(Barren, Hues.DarkGray)
            },
            {
                Inferno,
                New SatelliteTypeDescriptor(Inferno, Hues.Red)
            },
            {
                Cavernous,
                New SatelliteTypeDescriptor(Cavernous, Hues.LightGray)
            },
            {
                Ice,
                New SatelliteTypeDescriptor(Ice, Hues.White)
            }
        }
End Module
