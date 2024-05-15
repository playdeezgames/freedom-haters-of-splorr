Friend Class HueDescriptor
    ReadOnly Property Hue As Integer
    ReadOnly Property Name As String
    Sub New(hue As Integer, name As String)
        Me.Hue = hue
        Me.Name = name
    End Sub
End Class
