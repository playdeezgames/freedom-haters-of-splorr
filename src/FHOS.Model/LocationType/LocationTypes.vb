﻿Friend Module LocationTypes
    Friend Const Void = "Void"
    Friend Const VoidNorthArrow = "VoidNorthArrow"
    Friend Const VoidNorthEastArrow = "VoidNorthEastArrow"
    Friend Const VoidEastArrow = "VoidEastArrow"
    Friend Const VoidSouthEastArrow = "VoidSouthEastArrow"
    Friend Const VoidSouthArrow = "VoidSouthArrow"
    Friend Const VoidSouthWestArrow = "VoidSouthWestArrow"
    Friend Const VoidWestArrow = "VoidWestArrow"
    Friend Const VoidNorthWestArrow = "VoidNorthWestArrow"

    Friend Const BlueStar = "BlueStar"
    Friend Const BlueWhiteStar = "BlueWhiteStar"
    Friend Const YellowStar = "YellowStar"
    Friend Const OrangeStar = "OrangeStar"
    Friend Const RedStar = "RedStar"

    Friend Const RadiatedPlanet = "RadiatedPlanet"
    Friend Const ToxicPlanet = "ToxicPlanet"
    Friend Const VolcanicPlanet = "VolcanicPlanet"
    Friend Const BarrenPlanet = "BarrenPlanet"
    Friend Const DesertPlanet = "DesertPlanet"
    Friend Const TundraPlanet = "TundraPlanet"
    Friend Const AridPlanet = "AridPlanet"
    Friend Const OceanPlanet = "OceanPlanet"
    Friend Const TerranPlanet = "TerranPlanet"
    Friend Const InfernoPlanet = "InfernoPlanet"
    Friend Const TropicalPlanet = "TropicalPlanet"
    Friend Const GrasslandPlanet = "GrasslandPlanet"
    Friend Const CavernousPlanet = "CavernousPlanet"
    Friend Const GaiaPlanet = "GaiaPlanet"
    Friend Const SwampPlanet = "SwampPlanet"

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, LocationTypeDescriptor) =
        New Dictionary(Of String, LocationTypeDescriptor) From
        {
            {Void, New LocationTypeDescriptor("Empty Space", ChrW(0), DarkGray, Black)},
            {VoidNorthArrow, New LocationTypeDescriptor("Empty Space", ChrW(16), DarkGray, Black)},
            {VoidNorthEastArrow, New LocationTypeDescriptor("Empty Space", ChrW(17), DarkGray, Black)},
            {VoidEastArrow, New LocationTypeDescriptor("Empty Space", ChrW(18), DarkGray, Black)},
            {VoidSouthEastArrow, New LocationTypeDescriptor("Empty Space", ChrW(19), DarkGray, Black)},
            {VoidSouthArrow, New LocationTypeDescriptor("Empty Space", ChrW(20), DarkGray, Black)},
            {VoidSouthWestArrow, New LocationTypeDescriptor("Empty Space", ChrW(21), DarkGray, Black)},
            {VoidWestArrow, New LocationTypeDescriptor("Empty Space", ChrW(22), DarkGray, Black)},
            {VoidNorthWestArrow, New LocationTypeDescriptor("Empty Space", ChrW(23), DarkGray, Black)},
            {BlueStar, New LocationTypeDescriptor("Blue Star", ChrW(224), Hue.Blue, Black)},
            {BlueWhiteStar, New LocationTypeDescriptor("Blue-White Star", ChrW(224), Hue.LightBlue, Black)},
            {YellowStar, New LocationTypeDescriptor("Yellow Star", ChrW(224), Hue.Yellow, Black)},
            {OrangeStar, New LocationTypeDescriptor("Orange Star", ChrW(224), Hue.Orange, Black)},
            {RedStar, New LocationTypeDescriptor("Red Star", ChrW(224), Hue.Red, Black)},
            {RadiatedPlanet, New LocationTypeDescriptor("Radiated Planet", ChrW(225), Hue.Yellow, Black)},
            {ToxicPlanet, New LocationTypeDescriptor("Toxic Planet", ChrW(225), Hue.Purple, Black)},
            {VolcanicPlanet, New LocationTypeDescriptor("Volcanic Planet", ChrW(225), Hue.Orange, Black)},
            {BarrenPlanet, New LocationTypeDescriptor("Barren Planet", ChrW(225), Hue.DarkGray, Black)},
            {DesertPlanet, New LocationTypeDescriptor("Desert Planet", ChrW(225), Hue.Brown, Black)},
            {TundraPlanet, New LocationTypeDescriptor("Tundra Planet", ChrW(225), Hue.White, Black)},
            {AridPlanet, New LocationTypeDescriptor("Arid Planet", ChrW(225), Hue.Tan, Black)},
            {OceanPlanet, New LocationTypeDescriptor("Ocean Planet", ChrW(225), Hue.Blue, Black)},
            {TerranPlanet, New LocationTypeDescriptor("Terran Planet", ChrW(225), Hue.Green, Black)},
            {InfernoPlanet, New LocationTypeDescriptor("Inferno Planet", ChrW(225), Hue.Red, Black)},
            {TropicalPlanet, New LocationTypeDescriptor("Tropical Planet", ChrW(225), Hue.LightBlue, Black)},
            {GrasslandPlanet, New LocationTypeDescriptor("Grassland Planet", ChrW(225), Hue.LightGreen, Black)},
            {CavernousPlanet, New LocationTypeDescriptor("Cavernous Planet", ChrW(225), Hue.LightGray, Black)},
            {GaiaPlanet, New LocationTypeDescriptor("Gaia Planet", ChrW(225), Hue.Pink, Black)},
            {Swamp, New LocationTypeDescriptor("Gaia Planet", ChrW(225), Hue.Cyan, Black)}
        }
End Module