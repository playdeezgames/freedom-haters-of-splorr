Imports SPLORR.Game

Friend Class EnterInteriorInteractionModel
    Implements IInteractionModel

    Private ReadOnly actor As Persistence.IActor

    Public Sub New(actor As Persistence.IActor)
        Me.actor = actor
    End Sub

    Public Sub Perform() Implements IInteractionModel.Perform
        DoTurn(actor)
        With actor.State.Interactor.Properties.Interior
            SetLocation(actor, RNG.FromEnumerable(.Locations.Where(Function(x) x.IsEdge AndAlso x.Actor Is Nothing)))
        End With
        actor.State.Interactor = Nothing
    End Sub
End Class
