Public Interface IUniverseModel
    Sub Save(filename As String)
    Sub Load(filename As String)
    Sub Abandon()
    ReadOnly Property Generator As IUniverseGeneratorModel
    ReadOnly Property Settings As IUniverseSettingsModel
    ReadOnly Property State As IUniverseStateModel

    'TODO: state
    ReadOnly Property Board As IBoardModel
    ReadOnly Property Avatar As IAvatarModel
    ReadOnly Property Messages As IMessagesModel
    ReadOnly Property Turn As Integer

    'TODO: Pedia
    ReadOnly Property FactionList As IEnumerable(Of (Text As String, Faction As IFactionModel))
    ReadOnly Property StarSystems As IEnumerable(Of (Text As String, Place As IPlaceModel))
    ReadOnly Property Planets As IEnumerable(Of (Text As String, Place As IPlaceModel))
    ReadOnly Property Satellites As IEnumerable(Of (Text As String, Place As IPlaceModel))
End Interface
