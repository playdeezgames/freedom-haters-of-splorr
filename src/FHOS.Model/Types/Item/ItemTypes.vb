Friend Module ItemTypes
    Friend ReadOnly FuelScoop As String = NameOf(FuelScoop)
    Friend ReadOnly AtmosphericConcentrator As String = NameOf(AtmosphericConcentrator)
    Friend ReadOnly Scrap As String = NameOf(Scrap)
    Friend ReadOnly OxygenTank As String = NameOf(OxygenTank)
    Friend ReadOnly FuelRod As String = NameOf(FuelRod)
    Friend ReadOnly LifeSupport As String = NameOf(LifeSupport)
    Friend ReadOnly FuelSupply As String = NameOf(FuelSupply)

    Private Function CreateItemTypes() As IReadOnlyList(Of ItemTypeDescriptor)
        Dim result = New List(Of ItemTypeDescriptor) From {
            New FuelScoopItemTypeDescriptor(),
            New AtmosphericConcentratorItemTypeDescriptor(),
            New ScrapItemTypeDescriptor(),
            New OxygenTankDescriptor(),
            New FuelRodDescriptor(),
            New LifeSupportItemTypeDescriptor(MarkI),
            New FuelSupplyItemTypeDescriptor(MarkI)
        }
        Return result
    End Function

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, ItemTypeDescriptor) =
        CreateItemTypes().ToDictionary(Function(x) x.ItemType, Function(x) x)
    Friend Function MarkedDescriptor(itemType As String, mark As String) As ItemTypeDescriptor
        Return Descriptors($"{itemType}{mark}")
    End Function
End Module
