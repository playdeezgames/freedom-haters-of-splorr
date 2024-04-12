Public Interface IWorldModel
    Sub Save(filename As String)
    Sub Load(filename As String)
    Sub Abandon()
    Sub Embark()
    ReadOnly Property Board As IBoardModel
    ReadOnly Property Avatar As IAvatarModel
    ReadOnly Property GalacticDensityName As String
    ReadOnly Property GalacticAgeName As String
End Interface
