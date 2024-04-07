Public Interface IBoardModel
    Function GetCell(boardPosition As (X As Integer, Y As Integer)) As ICellModel
End Interface
