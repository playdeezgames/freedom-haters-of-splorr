Public Interface IUniverseModel
    Sub Save(filename As String)
    Sub Load(filename As String)
    Sub Abandon()
    Sub Embark()

    ReadOnly Property GalacticAge As IGalacticAgeModel

    Sub SetGalacticDensity(density As String)
    ReadOnly Property GalacticDensityName As String
    ReadOnly Property GalacticDensityOptions As IEnumerable(Of (Text As String, Item As String))

    ReadOnly Property Board As IBoardModel
    ReadOnly Property Avatar As IAvatarModel
End Interface
