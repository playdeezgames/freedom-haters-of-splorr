Friend Module CostumeTypes
    Friend ReadOnly MerchantVessel As String = NameOf(MerchantVessel)
    Friend ReadOnly MilitaryVessel As String = NameOf(MilitaryVessel)
    Friend ReadOnly Person As String = NameOf(Person)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, CostumeTypeDescriptor) =
        GenerateDescriptors().
        ToDictionary(Function(x) x.Name, Function(x) x)

    Private Function GenerateDescriptors() As IReadOnlyList(Of CostumeTypeDescriptor)
        Return New List(Of CostumeTypeDescriptor) From
        {
            New CostumeTypeDescriptor(
                MerchantVessel,
                {ChrW(132), ChrW(133), ChrW(134), ChrW(135)},
                DarkGray),
            New CostumeTypeDescriptor(
                MilitaryVessel,
                {ChrW(128), ChrW(129), ChrW(130), ChrW(131)},
                LightGray),
            New CostumeTypeDescriptor(
                Person,
                {ChrW(160), ChrW(160), ChrW(160), ChrW(160)},
                LightGray)
        }
    End Function
End Module
