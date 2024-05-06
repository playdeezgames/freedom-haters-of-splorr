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
                SatelliteTypes.BarrenMoon,
                1
            },
            {
                SatelliteTypes.RadiatedMoon,
                1
            },
            {
                SatelliteTypes.VolcanicMoon,
                1
            },
            {
                SatelliteTypes.InfernoMoon,
                1
            },
            {
                SatelliteTypes.CavernousMoon,
                1
            },
            {
                SatelliteTypes.IceMoon,
                1
            }
        }
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, PlanetTypeDescriptor) =
        New Dictionary(Of String, PlanetTypeDescriptor) From
        {
            {
                Radiated,
                New PlanetTypeDescriptor(
                    PlanetTypes.Radiated,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator)
            },
            {
                Toxic,
                New PlanetTypeDescriptor(
                    PlanetTypes.Toxic,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator)
            },
            {
                Volcanic,
                New PlanetTypeDescriptor(
                    PlanetTypes.Volcanic,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator)
            },
            {
                Barren,
                New PlanetTypeDescriptor(
                    PlanetTypes.Barren,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator)
            },
            {
                Desert,
                New PlanetTypeDescriptor(
                    PlanetTypes.Desert,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            },
            {
                Tundra,
                New PlanetTypeDescriptor(
                    PlanetTypes.Tundra,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            },
            {
                Arid,
                New PlanetTypeDescriptor(
                    PlanetTypes.Arid,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            },
            {
                Ocean,
                New PlanetTypeDescriptor(
                    PlanetTypes.Ocean,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            },
            {
                Terran,
                New PlanetTypeDescriptor(
                    PlanetTypes.Terran,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            },
            {
                Inferno,
                New PlanetTypeDescriptor(
                    PlanetTypes.Inferno,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator)
            },
            {
                Tropical,
                New PlanetTypeDescriptor(
                    PlanetTypes.Tropical,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            },
            {
                Grassland,
                New PlanetTypeDescriptor(
                    PlanetTypes.Grassland,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            },
            {
                Cavernous,
                New PlanetTypeDescriptor(
                    PlanetTypes.Cavernous,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator)
            },
            {
                Gaia,
                New PlanetTypeDescriptor(
                    PlanetTypes.Gaia,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            },
            {
                Swamp,
                New PlanetTypeDescriptor(
                    PlanetTypes.Swamp,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            }
        }
End Module
