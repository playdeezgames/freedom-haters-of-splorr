Public Interface IUniverseStateModel
    ReadOnly Property Board As IBoardModel
    Function GetLocation(boardPosition As (X As Integer, Y As Integer)) As ILocationModel
    ReadOnly Property Avatar As IAvatarModel
    ReadOnly Property Messages As IMessagesModel
    ReadOnly Property Turn As Integer
End Interface
