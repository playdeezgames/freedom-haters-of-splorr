Public Interface IWorldModel
    Sub Save(filename As String)
    Sub Load(filename As String)
    Sub Abandon()
    Sub Embark()
    Sub SetGalacticAge(age As String)
    Sub SetGalacticDensity(density As String)
    ReadOnly Property Board As IBoardModel
    ReadOnly Property Avatar As IAvatarModel
    ReadOnly Property GalacticDensityName As String
    ReadOnly Property GalacticAgeName As String
    ReadOnly Property GalacticDensityOptions As IEnumerable(Of (Text As String, Item As String))
    ReadOnly Property GalacticAgeOptions As IEnumerable(Of (Text As String, Item As String))
End Interface
