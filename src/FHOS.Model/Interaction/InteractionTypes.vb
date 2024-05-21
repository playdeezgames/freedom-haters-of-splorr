Friend Module InteractionTypes
    Friend ReadOnly GoodBye As String = NameOf(GoodBye)
    Friend ReadOnly RefillOxygen As String = NameOf(RefillOxygen)
    Friend ReadOnly Refuel As String = NameOf(Refuel)
    Friend ReadOnly SalvageScrap As String = NameOf(SalvageScrap)
    Friend ReadOnly SellScrap As String = NameOf(SellScrap)
    Friend ReadOnly EnterWormhole As String = NameOf(EnterWormhole)
    Friend ReadOnly EnterOrbit As String = NameOf(EnterOrbit)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, InteractionTypeDescriptor) =
        New List(Of InteractionTypeDescriptor) From
        {
            New GoodByeInteractionTypeDescriptor(),
            New RefillOxygenInteractionTypeDescriptor(),
            New SalvageScrapInteractionTypeDescriptor(),
            New SellScrapInteractionTypeDescriptor(),
            New RefuelInteractionTypeDescriptor(),
            New EnterWormholeInteractionTypeDescriptor(),
            New EnterOrbitInteractionTypeDescriptor()
        }.ToDictionary(Function(x) x.InteractionType, Function(x) x)
End Module
