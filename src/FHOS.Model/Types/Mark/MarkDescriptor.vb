Friend Class MarkDescriptor
    Sub New(
           mark As String,
           value As Integer,
           name As String,
           description As String)
        Me.Mark = mark
        Me.Name = name
        Me.Description = description
        Me.Value = value
    End Sub

    Public ReadOnly Property Mark As String
    Public ReadOnly Property Name As String
    Public ReadOnly Property Description As String
    Public ReadOnly Property Value As Integer
End Class
