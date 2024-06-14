Public Interface IGalacticDensityModel
    Sub SetDensity(density As String)
    ReadOnly Property CurrentName As String
    ReadOnly Property Current As String
    ReadOnly Property Options As IEnumerable(Of (Text As String, Item As String))
End Interface
