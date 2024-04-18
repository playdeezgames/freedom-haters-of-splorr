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
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, PlanetTypeDescriptor) =
        New Dictionary(Of String, PlanetTypeDescriptor) From
        {
            {Radiated, New PlanetTypeDescriptor(RadiatedPlanet)},
            {Toxic, New PlanetTypeDescriptor(ToxicPlanet)},
            {Volcanic, New PlanetTypeDescriptor(VolcanicPlanet)},
            {Barren, New PlanetTypeDescriptor(BarrenPlanet)},
            {Desert, New PlanetTypeDescriptor(DesertPlanet)},
            {Tundra, New PlanetTypeDescriptor(TundraPlanet)},
            {Arid, New PlanetTypeDescriptor(AridPlanet)},
            {Ocean, New PlanetTypeDescriptor(OceanPlanet)},
            {Terran, New PlanetTypeDescriptor(TerranPlanet)},
            {Inferno, New PlanetTypeDescriptor(InfernoPlanet)},
            {Tropical, New PlanetTypeDescriptor(TropicalPlanet)},
            {Grassland, New PlanetTypeDescriptor(GrasslandPlanet)},
            {Cavernous, New PlanetTypeDescriptor(CavernousPlanet)},
            {Gaia, New PlanetTypeDescriptor(GaiaPlanet)},
            {Swamp, New PlanetTypeDescriptor(Swamp)}
        }
End Module
