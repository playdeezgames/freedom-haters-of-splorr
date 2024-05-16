Public Interface IUniverseStateModel
    ReadOnly Property Board As IBoardModel
    ReadOnly Property Avatar As IAvatarModel
    ReadOnly Property Messages As IMessagesModel
    ReadOnly Property Turn As Integer
End Interface
