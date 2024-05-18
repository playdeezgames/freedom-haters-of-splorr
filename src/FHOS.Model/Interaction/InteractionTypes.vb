Friend Module InteractionTypes
    Friend ReadOnly GoodBye As String = NameOf(GoodBye)
    Friend ReadOnly RefillOxygen As String = NameOf(RefillOxygen)
    Friend ReadOnly SalvageScrap As String = NameOf(SalvageScrap)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, InteractionDescriptor) =
        New List(Of InteractionDescriptor) From
        {
            New GoodByeInteractionDescriptor(),
            New RefillOxygenInteractionDescriptor(),
            New SalvageScrapInteractionDescriptor()
        }.ToDictionary(Function(x) x.InteractionType, Function(x) x)
End Module
