Friend Module CostumeTypes
    Friend ReadOnly MerchantVessel As String = NameOf(MerchantVessel)
    Friend ReadOnly MilitaryVessel As String = NameOf(MilitaryVessel)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, CostumeTypeDescriptor) =
        New List(Of CostumeTypeDescriptor) From
        {
            New CostumeTypeDescriptor(MerchantVessel),
            New CostumeTypeDescriptor(MilitaryVessel)
        }.ToDictionary(Function(x) x.Name, Function(x) x)
End Module
