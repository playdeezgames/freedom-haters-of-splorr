Public Interface IUniversePediaModel
    ReadOnly Property FactionList As IEnumerable(Of IGroupModel)
    ReadOnly Property StarSystemList As IEnumerable(Of IGroupModel)
    ReadOnly Property PlanetList As IEnumerable(Of IGroupModel)
End Interface
