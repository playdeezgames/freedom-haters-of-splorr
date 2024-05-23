Friend Module PlanetTypes
    'stole from: https://masteroforion.fandom.com/wiki/Biomes
    Friend Const Radiated = "Radiated"
    Friend Const Toxic = "Toxic"
    Friend Const Volcanic = "Volcanic"
    Friend Const Barren = "Barren"
    Friend Const Desert = "Desert"
    Friend Const Tundra = "Tundra"
    Friend Const Arid = "Arid"
    Friend Const Ocean = "Ocean"
    Friend Const Terran = "Terran"
    Friend Const Inferno = "Inferno"
    Friend Const Tropical = "Tropical"
    Friend Const Grassland = "Grassland"
    Friend Const Cavernous = "Cavernous"
    Friend Const Gaia = "Gaia"
    Friend Const Swamp = "Swamp"
    Private Const DefaultSatelliteDistance = 5
    Private ReadOnly defaultSatelliteCountGenerator As IReadOnlyDictionary(Of Integer, Integer) =
        New Dictionary(Of Integer, Integer) From
        {
            {0, 1},
            {1, 2},
            {2, 3},
            {3, 2},
            {4, 1}
        }
    Private ReadOnly defaultSatelliteTypeGenerator As IReadOnlyDictionary(Of String, Integer) =
        New Dictionary(Of String, Integer) From
        {
            {
                SatelliteTypes.Barren,
                1
            },
            {
                SatelliteTypes.Radiated,
                1
            },
            {
                SatelliteTypes.Volcanic,
                1
            },
            {
                SatelliteTypes.Inferno,
                1
            },
            {
                SatelliteTypes.Cavernous,
                1
            },
            {
                SatelliteTypes.Ice,
                1
            }
        }
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, PlanetTypeDescriptor) =
        New Dictionary(Of String, PlanetTypeDescriptor) From
        {
            {
                Radiated,
                New PlanetTypeDescriptor(
                    PlanetTypes.Radiated, Hues.Yellow,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator)
            },
            {
                Toxic,
                New PlanetTypeDescriptor(
                    PlanetTypes.Toxic, Hues.Purple,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator)
            },
            {
                Volcanic,
                New PlanetTypeDescriptor(
                    PlanetTypes.Volcanic, Hues.Orange,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator)
            },
            {
                Barren,
                New PlanetTypeDescriptor(
                    PlanetTypes.Barren, Hues.DarkGray,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator)
            },
            {
                Desert,
                New PlanetTypeDescriptor(
                    PlanetTypes.Desert, Hues.Brown,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            },
            {
                Tundra,
                New PlanetTypeDescriptor(
                    PlanetTypes.Tundra, Hues.White,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            },
            {
                Arid,
                New PlanetTypeDescriptor(
                    PlanetTypes.Arid, Hues.Tan,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            },
            {
                Ocean,
                New PlanetTypeDescriptor(
                    PlanetTypes.Ocean, Hues.Blue,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            },
            {
                Terran,
                New PlanetTypeDescriptor(
                    PlanetTypes.Terran, Hues.Green,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            },
            {
                Inferno,
                New PlanetTypeDescriptor(
                    PlanetTypes.Inferno, Hues.Red,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator)
            },
            {
                Tropical,
                New PlanetTypeDescriptor(
                    PlanetTypes.Tropical, Hues.LightBlue,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            },
            {
                Grassland,
                New PlanetTypeDescriptor(
                    PlanetTypes.Grassland, Hues.LightGreen,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            },
            {
                Cavernous,
                New PlanetTypeDescriptor(
                    PlanetTypes.Cavernous, Hues.LightGray,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator)
            },
            {
                Gaia,
                New PlanetTypeDescriptor(
                    PlanetTypes.Gaia, Hues.Pink,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            },
            {
                Swamp,
                New PlanetTypeDescriptor(
                    PlanetTypes.Swamp, Hues.Cyan,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            }
        }
End Module
