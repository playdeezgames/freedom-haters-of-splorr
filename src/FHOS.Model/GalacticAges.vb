Friend Module GalacticAges
    Friend Const Young = "Young"
    Friend Const Average = "Average"
    Friend Const Old = "Old"
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, GalacticAgeDescriptor) =
        New Dictionary(Of String, GalacticAgeDescriptor) From
        {
            {Young, New GalacticAgeDescriptor("Young", 1, New Dictionary(Of String, Integer) From
            {
                {StarTypes.Blue, 5},
                {StarTypes.BlueWhite, 4},
                {StarTypes.Yellow, 3},
                {StarTypes.Orange, 2},
                {StarTypes.Red, 1}
            })},
            {Average, New GalacticAgeDescriptor("Average", 2, New Dictionary(Of String, Integer) From
            {
                {StarTypes.Blue, 1},
                {StarTypes.BlueWhite, 1},
                {StarTypes.Yellow, 1},
                {StarTypes.Orange, 1},
                {StarTypes.Red, 1}
            })},
            {Old, New GalacticAgeDescriptor("Old", 3, New Dictionary(Of String, Integer) From
            {
                {StarTypes.Blue, 1},
                {StarTypes.BlueWhite, 2},
                {StarTypes.Yellow, 3},
                {StarTypes.Orange, 4},
                {StarTypes.Red, 5}
            })}
        }
End Module
