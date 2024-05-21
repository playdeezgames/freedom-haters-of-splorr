Imports SPLORR.Game

Friend Class EnterWormholeInteractionModel
    Inherits InteractionModel

    Public Sub New(actor As Persistence.IActor)
        MyBase.New(actor)
    End Sub

    Public Overrides Sub Perform()
        Dim destinations = actor.State.Interactor.Properties.TargetActor.State.Location.GetNeighbors().Where(Function(x) x.Actor Is Nothing)
        If Not destinations.Any Then
            actor.Universe.Messages.Add("Destination Blocked!", ("Other end of the wormhole is blocked!", Hues.Black))
            actor.State.Interactor = Nothing
            Return
        End If
        Dim destination = RNG.FromEnumerable(destinations)
        SetLocation(actor, destination)
        actor.State.Interactor = Nothing
    End Sub
End Class
