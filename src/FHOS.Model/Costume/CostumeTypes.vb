Friend Module CostumeTypes
    Friend ReadOnly MerchantVessel As String = NameOf(MerchantVessel)
    Friend ReadOnly MilitaryVessel As String = NameOf(MilitaryVessel)
    Friend ReadOnly Person As String = NameOf(Person)

    Friend Function MakeCostume(costumeType As String, hue As Integer) As String
        Return $"{costumeType}{hue}"
    End Function

    Private ReadOnly glyphsTable As IReadOnlyDictionary(Of String, Char()) =
        New Dictionary(Of String, Char()) From
        {
            {MilitaryVessel, {ChrW(132), ChrW(133), ChrW(134), ChrW(135)}},
            {MerchantVessel, {ChrW(128), ChrW(129), ChrW(130), ChrW(131)}},
            {Person, {ChrW(160), ChrW(160), ChrW(160), ChrW(160)}}
        }

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, CostumeTypeDescriptor) =
        GenerateDescriptors().
        ToDictionary(Function(x) x.Name, Function(x) x)

    Private Function GenerateDescriptors() As IReadOnlyList(Of CostumeTypeDescriptor)
        Dim result = New List(Of CostumeTypeDescriptor)
        For Each glyph In glyphsTable
            For Each hue In Hues.Descriptors.Keys
                result.Add(New CostumeTypeDescriptor(MakeCostume(glyph.Key, hue), glyph.Value, hue))
            Next
        Next
        Return result
    End Function
End Module
