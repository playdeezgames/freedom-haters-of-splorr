Friend Class MarkDescriptor
    Sub New(mark As String, name As String, description As String)
        Me.Mark = mark
        Me.Name = name
        Me.Description = description
    End Sub

    Public ReadOnly Property Mark As String
    Public ReadOnly Property Name As String
    Public ReadOnly Property Description As String
End Class
