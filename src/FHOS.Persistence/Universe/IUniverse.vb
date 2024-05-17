Public Interface IUniverse
    ReadOnly Property Factory As IUniverseFactory
    ReadOnly Property Avatar As IAvatar
    Property Turn As Integer
    ReadOnly Property Messages As IMessages
    ReadOnly Property Places As IEnumerable(Of IPlace)
    Function GetPlacesOfType(placeType As String) As IEnumerable(Of IPlace)
    ReadOnly Property Factions As IEnumerable(Of IFaction)
    ReadOnly Property Actors As IEnumerable(Of IActor)
End Interface
