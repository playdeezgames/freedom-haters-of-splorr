Public Module SatelliteTypes
    Friend Const RadiatedMoon = "RadiatedMoon"
    Friend Const VolcanicMoon = "VolcanicMoon"
    Friend Const BarrenMoon = "BarrenMoon"
    Friend Const InfernoMoon = "InfernoMoon"
    Friend Const CavernousMoon = "CavernousMoon"
    Friend Const IceMoon = "IceMoon"

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, SatelliteDescriptor) =
        New Dictionary(Of String, SatelliteDescriptor) From
        {
            {
                RadiatedMoon,
                New SatelliteDescriptor(LocationTypes.RadiatedMoon)
            },
            {
                VolcanicMoon,
                New SatelliteDescriptor(LocationTypes.VolcanicMoon)
            },
            {
                BarrenMoon,
                New SatelliteDescriptor(LocationTypes.BarrenMoon)
            },
            {
                InfernoMoon,
                New SatelliteDescriptor(LocationTypes.InfernoMoon)
            },
            {
                CavernousMoon,
                New SatelliteDescriptor(LocationTypes.CavernousMoon)
            },
            {
                IceMoon,
                New SatelliteDescriptor(LocationTypes.IceMoon)
            }
        }
End Module
