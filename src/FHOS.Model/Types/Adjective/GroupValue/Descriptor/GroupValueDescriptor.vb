Friend Class GroupValueDescriptor
    Friend ReadOnly Property Name As String
    Friend ReadOnly Property Description As String
    Sub New(name As String, description As String)
        Me.name = name
        Me.description = description
    End Sub
End Class
