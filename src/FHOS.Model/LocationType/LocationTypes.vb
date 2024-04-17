Friend Module LocationTypes
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
            {RedStar, New LocationTypeDescriptor("Red Star", ChrW(224), Hue.Red, Black)}
        }
End Module
