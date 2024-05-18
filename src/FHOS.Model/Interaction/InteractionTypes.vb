Friend Module InteractionTypes
    Friend ReadOnly GoodBye As String = NameOf(GoodBye)
    Friend ReadOnly RefillOxygen As String = NameOf(RefillOxygen)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, InteractionDescriptor) =
        New List(Of InteractionDescriptor) From
        {
            New GoodByeInteractionDescriptor(),
            New RefillOxygenInteractionDescriptor()
        }.ToDictionary(Function(x) x.InteractionType, Function(x) x)
End Module
