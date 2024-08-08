Public Interface IUniverse
    ReadOnly Property Factory As IUniverseFactory
    ReadOnly Property Avatar As IAvatar
    Property Turn As Integer
    ReadOnly Property Groups As IEnumerable(Of IGroup)
    ReadOnly Property Actors As IEnumerable(Of IActor)
End Interface
