Friend Class SalvageScrapInteractionModel
    Inherits InteractionModel

    Public Sub New(actor As Persistence.IActor, scrap As Integer)
        MyBase.New(actor)
    End Sub

    Public Overrides Sub Perform()
        Dim interactor = actor.YokedActor(YokeTypes.Interactor)
        Dim scrap = If(interactor.Statistics(StatisticTypes.Scrap), 0)
        actor.Universe.Messages.Add("Salvage!", ($"You find {scrap} scrap!", Black))
        actor.Statistics(StatisticTypes.Scrap) += scrap
        Dim starSystemGroup = actor.Location.Map.YokedGroup(GroupTypes.StarSystem)
        starSystemGroup.Statistics(StatisticTypes.Scrap) -= 1
        interactor.Recycle()
        actor.YokedActor(YokeTypes.Interactor) = Nothing
    End Sub
End Class
