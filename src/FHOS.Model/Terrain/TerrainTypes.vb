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
            {Void, New TerrainTypeDescriptor(ChrW(0), Black, Black)},
            {BlueStar, New TerrainTypeDescriptor("*"c, Hue.Blue, Black)},
            {BlueWhiteStar, New TerrainTypeDescriptor("*"c, Hue.LightBlue, Black)},
            {YellowStar, New TerrainTypeDescriptor("*"c, Hue.Yellow, Black)},
            {OrangeStar, New TerrainTypeDescriptor("*"c, Hue.Orange, Black)},
            {RedStar, New TerrainTypeDescriptor("*"c, Hue.Red, Black)}
        }
End Module
