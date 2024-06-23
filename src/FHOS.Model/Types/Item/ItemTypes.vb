Friend Module ItemTypes
    Friend ReadOnly FuelScoop As String = NameOf(FuelScoop)
    Friend ReadOnly AtmosphericConcentrator As String = NameOf(AtmosphericConcentrator)
    Friend ReadOnly Scrap As String = NameOf(Scrap)
    Friend ReadOnly OxygenTank As String = NameOf(OxygenTank)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, ItemTypeDescriptor) =
        New List(Of ItemTypeDescriptor) From
        {
            New FuelScoopItemTypeDescriptor(),
            New AtmosphericConcentratorItemTypeDescriptor(),
            New ScrapItemTypeDescriptor(),
            New OxygenTankDescriptor()
        }.ToDictionary(Function(x) x.ItemType, Function(x) x)
End Module
