Friend Class SalvageScrapInteractionModel
    Inherits InteractionModel

    Public Sub New(actor As Persistence.IActor)
        MyBase.New(actor)
    End Sub

    Public Overrides Sub Perform()
        Dim interactor = actor.Interactor
        Dim scrap = If(interactor.Statistics(StatisticTypes.Scrap), 0)
        Dim loot = interactor.Inventory.Items.ToList
        For Each item In loot
            interactor.Inventory.Remove(item)
            actor.Inventory.Add(item)
        Next
        'loot summary

        actor.Universe.Messages.Add("Salvage!", ($"You find {scrap} scrap!", Black))
        actor.Statistics(StatisticTypes.Scrap) += scrap
        Dim starSystemGroup = actor.Location.Map.YokedGroup(GroupTypes.StarSystem)
        starSystemGroup.Statistics(StatisticTypes.Scrap) -= 1
        interactor.Recycle()
        actor.ClearInteractor()
    End Sub
End Class
