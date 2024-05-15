Public Interface IUniverseModel
    Sub Save(filename As String)
    Sub Load(filename As String)
    Sub Abandon()
    Sub Embark()

    Sub Generate()
    ReadOnly Property GenerationStepsRemaining As Integer
    ReadOnly Property GenerationStepsCompleted As Integer
    ReadOnly Property DoneGenerating As Boolean

    ReadOnly Property GalacticAge As IGalacticAgeModel
    ReadOnly Property GalacticDensity As IGalacticDensityModel
    ReadOnly Property StartingWealth As IStartingWealthLevelModel
    ReadOnly Property FactionCount As IFactionCountModel

    ReadOnly Property Board As IBoardModel
    ReadOnly Property Avatar As IAvatarModel
    ReadOnly Property Messages As IMessagesModel
    ReadOnly Property FactionList As IEnumerable(Of (String, IFactionModel))
    ReadOnly Property StarSystems As IEnumerable(Of (Text As String, Place As IPlaceModel))
    ReadOnly Property Planets As IEnumerable(Of (Text As String, Place As IPlaceModel))
    ReadOnly Property Satellites As IEnumerable(Of (Text As String, Place As IPlaceModel))
    ReadOnly Property Turn As Integer
End Interface
