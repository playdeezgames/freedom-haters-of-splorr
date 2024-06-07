Friend Class SalvageScrapInteractionModel
    Inherits InteractionModel

    Public Sub New(actor As Persistence.IActor, scrap As Integer)
        MyBase.New(actor)
    End Sub

    Public Overrides Sub Perform()
        Dim scrap = If(actor.State.Interactor.Statistics(StatisticTypes.Scrap), 0)
        actor.Universe.Messages.Add("Salvage!", ($"You find {scrap} scrap!", Black))
        actor.Statistics(StatisticTypes.Scrap) += scrap
        actor.State.Interactor.Recycle()
        actor.State.Interactor = Nothing
    End Sub
End Class
