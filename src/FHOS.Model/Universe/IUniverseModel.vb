Public Interface IUniverseModel
    Sub Save(filename As String)
    Sub Load(filename As String)
    Sub Abandon()
    Sub Embark()
    ReadOnly Property GalacticAge As IGalacticAgeModel
    ReadOnly Property GalacticDensity As IGalacticDensityModel

    ReadOnly Property Board As IBoardModel
    ReadOnly Property Avatar As IAvatarModel
    ReadOnly Property Messages As IMessagesModel
End Interface
