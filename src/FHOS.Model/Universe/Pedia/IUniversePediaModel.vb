Public Interface IUniversePediaModel
    ReadOnly Property Factions As IEnumerable(Of IGroupModel)
    ReadOnly Property StarSystems As IEnumerable(Of IGroupModel)
    ReadOnly Property PlanetVicinities As IEnumerable(Of IGroupModel)
    ReadOnly Property Satellites As IEnumerable(Of IGroupModel)
End Interface
