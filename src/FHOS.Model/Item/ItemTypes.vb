Friend Module ItemTypes
    Friend ReadOnly FuelScoop As String = NameOf(FuelScoop)
    Friend ReadOnly AtmosphericConcentrator As String = NameOf(AtmosphericConcentrator)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, ItemTypeDescriptor) =
        New List(Of ItemTypeDescriptor) From
        {
            New FuelScoopDescriptor(),
            New AtmosphericConcentrator()
        }.ToDictionary(Function(x) x.ItemType, Function(x) x)
End Module
