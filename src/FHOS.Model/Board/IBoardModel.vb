Public Interface IBoardModel
    Function GetLocation(boardPosition As (X As Integer, Y As Integer)) As ILocationModel
End Interface
