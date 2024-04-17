Friend Class StarTypeDescriptor
    Sub New(starTypeName As String, locationType As String, generatorWeight As Integer)
        Me.StarTypeName = starTypeName
        Me.LocationType = locationType
        Me.GeneratorWeight = generatorWeight
    End Sub
    Friend ReadOnly Property StarTypeName As String
    Friend ReadOnly Property LocationType As String
    Friend ReadOnly Property GeneratorWeight As Integer
End Class
