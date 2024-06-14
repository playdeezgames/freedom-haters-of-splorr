Imports FHOS.Persistence

Friend Class UniverseStateModel
    Implements IUniverseStateModel

    Private ReadOnly universe As IUniverse

    Public Sub New(universe As IUniverse)
        Me.universe = universe
    End Sub

    Friend Shared Function FromUniverse(universe As IUniverse) As IUniverseStateModel
        Return New UniverseStateModel(universe)
    End Function

    Public ReadOnly Property Board As IBoardModel Implements IUniverseStateModel.Board
        Get
            Return BoardModel.FromUniverse(universe)
        End Get
    End Property

    Public ReadOnly Property Avatar As IAvatarModel Implements IUniverseStateModel.Avatar
        Get
            Return AvatarModel.FromActor(universe.Avatar.Actor)
        End Get
    End Property

    Public ReadOnly Property Messages As IMessagesModel Implements IUniverseStateModel.Messages
        Get
            Return MessagesModel.FromUniverse(universe)
        End Get
    End Property

    Public ReadOnly Property Turn As Integer Implements IUniverseStateModel.Turn
        Get
            Return universe.Turn
        End Get
    End Property

    Public Function GetLocation(boardPosition As (X As Integer, Y As Integer)) As ILocationModel Implements IUniverseStateModel.GetLocation
        Return LocationModel.FromBoardPosition(universe, boardPosition)
    End Function
End Class
