Imports SPLORR.Game

Friend Module StarTypes
    Friend Const Blue = "Blue"
    Friend Const BlueWhite = "BlueWhite"
    Friend Const Yellow = "Yellow"
    Friend Const Orange = "Orange"
    Friend Const Red = "Red"
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, StarTypeDescriptor) =
        New Dictionary(Of String, StarTypeDescriptor) From
        {
            {StarTypes.Blue, New StarTypeDescriptor("Blue Star", TerrainTypes.BlueStar, 1)},
            {StarTypes.BlueWhite, New StarTypeDescriptor("Blue-White Star", TerrainTypes.BlueWhiteStar, 1)},
            {StarTypes.Yellow, New StarTypeDescriptor("Yellow Star", TerrainTypes.YellowStar, 1)},
            {StarTypes.Orange, New StarTypeDescriptor("Orange Star", TerrainTypes.OrangeStar, 1)},
            {StarTypes.Red, New StarTypeDescriptor("Red Star", TerrainTypes.RedStar, 1)}
        }
    Private ReadOnly starTypeGenerator As IReadOnlyDictionary(Of String, Integer) =
        Descriptors.ToDictionary(Function(x) x.Key, Function(x) x.Value.GeneratorWeight)
    Friend Function GenerateStarType() As String
        Return RNG.FromGenerator(starTypeGenerator)
    End Function
End Module
