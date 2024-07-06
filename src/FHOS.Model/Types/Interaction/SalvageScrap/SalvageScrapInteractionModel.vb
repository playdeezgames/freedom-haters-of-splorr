Friend Class SalvageScrapInteractionModel
    Inherits InteractionModel

    Public Sub New(actor As Persistence.IActor)
        MyBase.New(actor, AddressOf DoSalvageScrap)
    End Sub

    Private Shared Sub DoSalvageScrap(actor As Persistence.IActor)
        Dim interactor = actor.Interactor
        Dim loot = interactor.Inventory.Items.ToList
        For Each item In loot
            interactor.Inventory.Remove(item)
            actor.Inventory.Add(item)
        Next
        'loot summary
        Dim lines As New List(Of (Text As String, Hue As Integer)) From
            {
                ("You find:", Black)
            }
        lines.AddRange(loot.GroupBy(Function(x) x.Descriptor.Name).Select(Function(x) ($"{x.Count} {x.Key}", Black)))
        actor.Universe.Messages.Add("Salvage!", lines.ToArray)
        Dim starSystemGroup = actor.Location.Map.YokedGroup(GroupTypes.StarSystem)
        starSystemGroup.Statistics(StatisticTypes.Scrap) -= 1
        interactor.Recycle()
        actor.ClearInteractor()
    End Sub
End Class
