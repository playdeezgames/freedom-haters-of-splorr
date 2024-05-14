Imports FHOS.Persistence

Friend Class BoardModel
    Implements IBoardModel

    Private ReadOnly universe As Persistence.IUniverse

    Protected Sub New(universe As Persistence.IUniverse)
        Me.universe = universe
    End Sub

    Friend Shared Function FromUniverse(universe As IUniverse) As IBoardModel
        Return New BoardModel(universe)
    End Function

    Public Function GetLocation(boardPosition As (X As Integer, Y As Integer)) As ILocationModel Implements IBoardModel.GetLocation
        Return New LocationModel(universe, boardPosition)
    End Function
End Class
