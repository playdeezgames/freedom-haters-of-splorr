Friend Module FactionCounts
    Friend Const Two = 2
    Friend Const Three = 3
    Friend Const Four = 4
    Friend Const Five = 5
    Friend Const Six = 6

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of Integer, FactionCountDescriptor) =
        New List(Of FactionCountDescriptor) From
        {
            New FactionCountDescriptor(Two, "Two"),
            New FactionCountDescriptor(Three, "Three"),
            New FactionCountDescriptor(Four, "Four"),
            New FactionCountDescriptor(Five, "Five"),
            New FactionCountDescriptor(Six, "Six")
        }.ToDictionary(Function(x) x.factionCount, Function(x) x)
End Module
