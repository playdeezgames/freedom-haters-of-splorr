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
                    RadiatedPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator)
            },
            {
                Toxic,
                New PlanetTypeDescriptor(
                    ToxicPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator)
            },
            {
                Volcanic,
                New PlanetTypeDescriptor(
                    VolcanicPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator)
            },
            {
                Barren,
                New PlanetTypeDescriptor(
                    BarrenPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator)
            },
            {
                Desert,
                New PlanetTypeDescriptor(
                    DesertPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            },
            {
                Tundra,
                New PlanetTypeDescriptor(
                    TundraPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            },
            {
                Arid,
                New PlanetTypeDescriptor(
                    AridPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            },
            {
                Ocean,
                New PlanetTypeDescriptor(
                    OceanPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            },
            {
                Terran,
                New PlanetTypeDescriptor(
                    TerranPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            },
            {
                Inferno,
                New PlanetTypeDescriptor(
                    InfernoPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator)
            },
            {
                Tropical,
                New PlanetTypeDescriptor(
                    TropicalPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            },
            {
                Grassland,
                New PlanetTypeDescriptor(
                    GrasslandPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            },
            {
                Cavernous,
                New PlanetTypeDescriptor(
                    CavernousPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator)
            },
            {
                Gaia,
                New PlanetTypeDescriptor(
                    GaiaPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            },
            {
                Swamp,
                New PlanetTypeDescriptor(
                    SwampPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    defaultSatelliteCountGenerator,
                    canRefillOxygen:=True)
            }
        }
End Module
