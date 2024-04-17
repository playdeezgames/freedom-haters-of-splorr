Friend Class BoardModel
    Implements IBoardModel

    Private ReadOnly universe As Persistence.IUniverse

    Public Sub New(universe As Persistence.IUniverse)
        Me.universe = universe
    End Sub

    Public Function GetLocation(boardPosition As (X As Integer, Y As Integer)) As ILocationModel Implements IBoardModel.GetLocation
        Return New LocationModel(universe, boardPosition)
    End Function
End Class
