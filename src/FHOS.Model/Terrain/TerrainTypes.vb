Friend Module TerrainTypes
    Friend Const Void = "Void"
    Friend Const BlueStar = "BlueStar"
    Friend Const BlueWhiteStar = "BlueWhiteStar"
    Friend Const YellowStar = "YellowStar"
    Friend Const OrangeStar = "OrangeStar"
    Friend Const RedStar = "RedStar"
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, TerrainTypeDescriptor) =
        New Dictionary(Of String, TerrainTypeDescriptor) From
        {
            {Void, New TerrainTypeDescriptor("Empty Space", ChrW(0), Black, Black)},
            {BlueStar, New TerrainTypeDescriptor("Blue Star", "*"c, Hue.Blue, Black)},
            {BlueWhiteStar, New TerrainTypeDescriptor("Blue-White Star", "*"c, Hue.LightBlue, Black)},
            {YellowStar, New TerrainTypeDescriptor("Yellow Star", "*"c, Hue.Yellow, Black)},
            {OrangeStar, New TerrainTypeDescriptor("Orange Star", "*"c, Hue.Orange, Black)},
            {RedStar, New TerrainTypeDescriptor("Red Star", "*"c, Hue.Red, Black)}
        }
End Module
