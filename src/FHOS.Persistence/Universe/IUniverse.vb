Public Interface IUniverse
    ReadOnly Property Factory As IUniverseFactory
    ReadOnly Property Avatar As IAvatar
    Property Turn As Integer
    ReadOnly Property Messages As IMessages
    ReadOnly Property Factions As IEnumerable(Of IFaction)
    ReadOnly Property Actors As IEnumerable(Of IActor)
End Interface
