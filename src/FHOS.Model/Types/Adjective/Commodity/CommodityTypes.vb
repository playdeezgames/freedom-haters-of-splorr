Friend Module CommodityTypes
    Friend ReadOnly Production As String = NameOf(Production)
    Friend ReadOnly Metal As String = NameOf(Metal)
    Friend ReadOnly Oxygen As String = NameOf(Oxygen)
    Friend ReadOnly Fuel As String = NameOf(Fuel)
    Friend ReadOnly Hype As String = NameOf(Hype)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, CommodityTypeDescriptor) =
        New List(Of CommodityTypeDescriptor) From
        {
            New ProductionCommodityTypeDescriptor(),
            New MetalCommodityTypeDescriptor(),
            New OxygenCommodityTypeDescriptor(),
            New FuelCommodityTypeDescriptor(),
            New HypeCommodityTypeDescriptor()
        }.ToDictionary(Function(x) x.CommodityType, Function(x) x)
End Module
