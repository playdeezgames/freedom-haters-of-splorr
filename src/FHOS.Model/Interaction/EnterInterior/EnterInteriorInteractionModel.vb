Imports SPLORR.Game

Friend Class EnterInteriorInteractionModel
    Implements IInteractionModel

    Private ReadOnly actor As Persistence.IActor

    Public Sub New(actor As Persistence.IActor)
        Me.actor = actor
    End Sub

    Public Sub Perform() Implements IInteractionModel.Perform
        DoTurn(actor)
        With actor.Interactor.Properties.Interior
            .YokedGroup(YokeTypes.StarSystem).Statistics(StatisticTypes.VisitCount) += 1
            SetLocation(actor, RNG.FromEnumerable(.Locations.Where(Function(x) x.Flags(FlagTypes.IsEdge) AndAlso x.Actor Is Nothing)))
        End With
        actor.ClearInteractor()
    End Sub
End Class
