Friend Module InteractionTypes
    Friend ReadOnly GoodBye As String = NameOf(GoodBye)
    Friend ReadOnly RefillOxygen As String = NameOf(RefillOxygen)
    Friend ReadOnly Refuel As String = NameOf(Refuel)
    Friend ReadOnly SalvageScrap As String = NameOf(SalvageScrap)
    Friend ReadOnly SellScrap As String = NameOf(SellScrap)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, InteractionDescriptor) =
        New List(Of InteractionDescriptor) From
        {
            New GoodByeInteractionDescriptor(),
            New RefillOxygenInteractionDescriptor(),
            New SalvageScrapInteractionDescriptor(),
            New SellScrapInteractionDescriptor(),
            New RefuelInteractionDescriptor()
        }.ToDictionary(Function(x) x.InteractionType, Function(x) x)
End Module
