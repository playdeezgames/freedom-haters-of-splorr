Friend Class SellScrapInteractionModel
    Implements IInteractionModel

    Private ReadOnly actor As Persistence.IActor

    Public Sub New(actor As Persistence.IActor)
        Me.actor = actor
    End Sub

    Public Sub Perform() Implements IInteractionModel.Perform
        Dim scrap = actor.State.Scrap
        actor.State.Scrap = 0
        actor.State.Wallet.CurrentValue += scrap
        actor.Universe.Messages.Add(
            "Sell Scrap!",
            ($"Gain {scrap} Jools!", Black))
    End Sub
End Class
