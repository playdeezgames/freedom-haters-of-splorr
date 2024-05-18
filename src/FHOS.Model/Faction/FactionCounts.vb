Friend Module FactionCounts
    Friend Const Two = 2
    Friend Const Three = 3
    Friend Const Four = 4
    Friend Const Five = 5
    Friend Const Six = 6
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of Integer, FactionCountDescriptor) =
        New Dictionary(Of Integer, FactionCountDescriptor) From
        {
            {Two, New FactionCountDescriptor(Two, "Two")},
            {Three, New FactionCountDescriptor(Three, "Three")},
            {Four, New FactionCountDescriptor(Four, "Four")},
            {Five, New FactionCountDescriptor(Five, "Five")},
            {Six, New FactionCountDescriptor(Six, "Six")}
        }
End Module
