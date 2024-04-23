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
                    defaultSatelliteTypeGenerator)
            },
            {
                Toxic,
                New PlanetTypeDescriptor(
                    ToxicPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator)
            },
            {
                Volcanic,
                New PlanetTypeDescriptor(
                    VolcanicPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator)
            },
            {
                Barren,
                New PlanetTypeDescriptor(
                    BarrenPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator)
            },
            {
                Desert,
                New PlanetTypeDescriptor(
                    DesertPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    canRefillOxygen:=True)
            },
            {
                Tundra,
                New PlanetTypeDescriptor(
                    TundraPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    canRefillOxygen:=True)
            },
            {
                Arid,
                New PlanetTypeDescriptor(
                    AridPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    canRefillOxygen:=True)
            },
            {
                Ocean,
                New PlanetTypeDescriptor(
                    OceanPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    canRefillOxygen:=True)
            },
            {
                Terran,
                New PlanetTypeDescriptor(
                    TerranPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    canRefillOxygen:=True)
            },
            {
                Inferno,
                New PlanetTypeDescriptor(
                    InfernoPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator)
            },
            {
                Tropical,
                New PlanetTypeDescriptor(
                    TropicalPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    canRefillOxygen:=True)
            },
            {
                Grassland,
                New PlanetTypeDescriptor(
                    GrasslandPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    canRefillOxygen:=True)
            },
            {
                Cavernous,
                New PlanetTypeDescriptor(
                    CavernousPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator)
            },
            {
                Gaia,
                New PlanetTypeDescriptor(
                    GaiaPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    canRefillOxygen:=True)
            },
            {
                Swamp,
                New PlanetTypeDescriptor(
                    SwampPlanet,
                    DefaultSatelliteDistance,
                    defaultSatelliteTypeGenerator,
                    canRefillOxygen:=True)
            }
        }
End Module
