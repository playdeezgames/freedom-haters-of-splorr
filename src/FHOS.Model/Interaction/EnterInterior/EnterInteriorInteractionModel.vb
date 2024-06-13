Imports SPLORR.Game

Friend Class EnterInteriorInteractionModel
    Implements IInteractionModel

    Private ReadOnly actor As Persistence.IActor

    Public Sub New(actor As Persistence.IActor)
        Me.actor = actor
    End Sub

    Public Sub Perform() Implements IInteractionModel.Perform
        DoTurn(actor)
        With actor.Interactor.Interior
            Dim starSystemGroup = .YokedGroup(YokeTypes.StarSystem)
            If starSystemGroup IsNot Nothing Then
                actor.SetStarSystem(starSystemGroup)
            End If
            SetLocation(actor, RNG.FromEnumerable(.Locations.Where(Function(x) x.Flags(FlagTypes.IsEdge) AndAlso x.Actor Is Nothing)))
        End With
        actor.ClearInteractor()
    End Sub
End Class
