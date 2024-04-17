Public Interface IGalacticDensityModel
    Sub SetGalacticDensity(density As String)
    ReadOnly Property GalacticDensityName As String
    ReadOnly Property GalacticDensityOptions As IEnumerable(Of (Text As String, Item As String))
    ReadOnly Property Value As String
End Interface
