Friend Class SellScrapInteractionModel
    Inherits InteractionModel

    Public Sub New(actor As Persistence.IActor)
        MyBase.New(actor)
    End Sub

    Public Overrides Sub Perform()
        Dim scrap = If(actor.Statistics(StatisticTypes.Scrap), 0)
        actor.Statistics(StatisticTypes.Scrap) = 0
        actor.State.Wallet.CurrentValue += scrap
        actor.Universe.Messages.Add(
            "Sell Scrap!",
            ($"Gain {scrap} Jools!", Black))
    End Sub
End Class
