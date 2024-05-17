Imports FHOS.Persistence

Friend Class MapTypeDescriptor
    ReadOnly Property MapType As String
    ReadOnly Property Size As (Columns As Integer, Rows As Integer)
    ReadOnly Property DefaultLocationType As String
    Function CreateMap(mapName As String, universe As IUniverse) As IMap
        Return universe.Factory.CreateMap(mapName, MapType, Size.Columns, Size.Rows, DefaultLocationType)
    End Function
    Sub New(
           mapType As String,
           size As (Columns As Integer, Rows As Integer),
           defaultLocationType As String)
        Me.MapType = mapType
        Me.Size = size
        Me.DefaultLocationType = defaultLocationType
    End Sub
End Class
