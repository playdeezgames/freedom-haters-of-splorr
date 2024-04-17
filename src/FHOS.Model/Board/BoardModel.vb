Friend Class BoardModel
    Implements IBoardModel

    Private ReadOnly world As Persistence.IUniverse

    Public Sub New(world As Persistence.IUniverse)
        Me.world = world
    End Sub

    Public Function GetLocation(boardPosition As (X As Integer, Y As Integer)) As ILocationModel Implements IBoardModel.GetLocation
        Return New LocationModel(world, boardPosition)
    End Function
End Class
