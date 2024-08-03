Friend Module ItemTypes
    Friend ReadOnly AtmosphericConcentrator As String = NameOf(AtmosphericConcentrator)
    Friend ReadOnly Delivery As String = NameOf(Delivery)
    Friend ReadOnly FuelRod As String = NameOf(FuelRod)
    Friend ReadOnly FuelScoop As String = NameOf(FuelScoop)
    Friend ReadOnly FuelSupply As String = NameOf(FuelSupply)
    Friend ReadOnly LifeSupport As String = NameOf(LifeSupport)
    Friend ReadOnly OxygenTank As String = NameOf(OxygenTank)
    Friend ReadOnly Scrap As String = NameOf(Scrap)
    Friend Function MarkedType(itemType As String, markType As String) As String
        Return $"{itemType}{markType}"
    End Function

    Private Function CreateItemTypes() As IReadOnlyList(Of ItemTypeDescriptor)
        Dim result = New List(Of ItemTypeDescriptor) From {
            New FuelScoopItemTypeDescriptor(),
            New AtmosphericConcentratorItemTypeDescriptor(),
            New ScrapItemTypeDescriptor(),
            New OxygenTankDescriptor(),
            New FuelRodDescriptor(),
            New DeliveryDescriptor()
        }
        For Each markType In Marks.Descriptors.Keys
            result.Add(New LifeSupportItemTypeDescriptor(markType))
            result.Add(New FuelSupplyItemTypeDescriptor(markType))
        Next
        Return result
    End Function

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, ItemTypeDescriptor) =
        CreateItemTypes().ToDictionary(Function(x) x.ItemType, Function(x) x)
    Friend Function MarkedDescriptor(itemType As String, mark As String) As ItemTypeDescriptor
        Return Descriptors($"{itemType}{mark}")
    End Function
End Module
