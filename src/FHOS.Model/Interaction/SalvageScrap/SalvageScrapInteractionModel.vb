Friend Class SalvageScrapInteractionModel
    Inherits InteractionModel

    Public Sub New(actor As Persistence.IActor, scrap As Integer)
        MyBase.New(actor)
    End Sub

    Public Overrides Sub Perform()
        Dim scrap = If(actor.YokedActor(YokeTypes.Interactor).Statistics(StatisticTypes.Scrap), 0)
        actor.Universe.Messages.Add("Salvage!", ($"You find {scrap} scrap!", Black))
        actor.Statistics(StatisticTypes.Scrap) += scrap
        actor.YokedActor(YokeTypes.Interactor).Recycle()
        actor.YokedActor(YokeTypes.Interactor) = Nothing
    End Sub
End Class
