Friend Class StarTypeDescriptor
    Sub New(starTypeName As String, terrainType As String, generatorWeight As Integer)
        Me.StarTypeName = starTypeName
        Me.TerrainType = terrainType
        Me.GeneratorWeight = generatorWeight
    End Sub
    Friend ReadOnly Property StarTypeName As String
    Friend ReadOnly Property TerrainType As String
    Friend ReadOnly Property GeneratorWeight As Integer
End Class
