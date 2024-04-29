Public Interface IUniverseModel
    Sub Save(filename As String)
    Sub Load(filename As String)
    Sub Abandon()
    Sub Embark()
    Sub Generate()
    ReadOnly Property GenerationStepsRemaining As Integer
    ReadOnly Property GenerationStepsCompleted As Integer
    ReadOnly Property GalacticAge As IGalacticAgeModel
    ReadOnly Property GalacticDensity As IGalacticDensityModel
    ReadOnly Property StartingWealth As IStartingWealthLevelModel

    ReadOnly Property Board As IBoardModel
    ReadOnly Property Avatar As IAvatarModel
    ReadOnly Property Messages As IMessagesModel
    ReadOnly Property DoneGenerating As Boolean
End Interface
