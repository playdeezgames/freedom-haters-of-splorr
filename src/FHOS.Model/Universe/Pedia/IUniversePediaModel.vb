Public Interface IUniversePediaModel
    ReadOnly Property FactionList As IEnumerable(Of IGroupModel)
    ReadOnly Property StarSystemList As IEnumerable(Of IGroupModel)
    ReadOnly Property PlanetVicinityList As IEnumerable(Of IGroupModel)
End Interface
