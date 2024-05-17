Friend Class MapTypeDescriptor
    ReadOnly Property MapType As String
    ReadOnly Property Size As (Columns As Integer, Rows As Integer)
    ReadOnly Property DefaultLocationType As String
    Sub New(
           mapType As String,
           size As (Columns As Integer, Rows As Integer),
           defaultLocationType As String)
        Me.MapType = mapType
        Me.Size = size
        Me.DefaultLocationType = defaultLocationType
    End Sub
End Class
