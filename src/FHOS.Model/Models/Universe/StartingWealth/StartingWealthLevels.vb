Friend Module StartingWealthLevels
    Friend Const VeryPoor = "VeryPoor"
    Friend Const Poor = "Poor"
    Friend Const Middle = "Middle"
    Friend Const Rich = "Rich"
    Friend Const VeryRich = "VeryRich"
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, StartingWealthLevelDescriptor) =
        New Dictionary(Of String, StartingWealthLevelDescriptor) From
        {
            {
                VeryPoor,
                New StartingWealthLevelDescriptor(
                    "Very Poor",
                    1,
                    Enumerable.Range(0, 1).ToDictionary(Function(x) x, Function(x) 1),
                    -999)
            },
            {
                Poor,
                New StartingWealthLevelDescriptor(
                    "Poor",
                    500,
                    Enumerable.Range(450, 100).ToDictionary(Function(x) x, Function(x) 1),
                    -999)
            },
            {
                Middle,
                New StartingWealthLevelDescriptor(
                    "Middle Class",
                    1000,
                    Enumerable.Range(450, 100).ToDictionary(Function(x) x * 2, Function(x) 1),
                    -999)
            },
            {
                Rich,
                New StartingWealthLevelDescriptor(
                    "Rich",
                    5000,
                    Enumerable.Range(450, 100).ToDictionary(Function(x) x * 10, Function(x) 1),
                    -499)
            },
            {
                VeryRich,
                New StartingWealthLevelDescriptor(
                    "Very Rich",
                    10000,
                    Enumerable.Range(450, 100).ToDictionary(Function(x) x * 20, Function(x) 1),
                    0)
            }
        }
End Module
