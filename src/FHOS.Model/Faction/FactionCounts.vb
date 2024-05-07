Friend Module FactionCounts
    Friend Const Two = 2
    Friend Const Three = 3
    Friend Const Four = 4
    Friend Const Five = 5
    Friend Const Six = 6
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of Integer, FactionCountDescriptor) =
        New Dictionary(Of Integer, FactionCountDescriptor) From
        {
            {Two, New FactionCountDescriptor(2, "Two")},
            {Three, New FactionCountDescriptor(2, "Three")},
            {Four, New FactionCountDescriptor(2, "Four")},
            {Five, New FactionCountDescriptor(2, "Five")},
            {Six, New FactionCountDescriptor(2, "Six")}
        }
End Module
