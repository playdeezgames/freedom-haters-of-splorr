Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class SalvageScrapInteractionTypeDescriptor
    Inherits InteractionTypeDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.SalvageScrap)
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.Interactor.Descriptor.CanSalvage
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New InteractionModel(actor, Sub(a)
                                               Dim interactor = a.Interactor
                                               Dim loot = interactor.Inventory.Items.ToList
                                               For Each item In loot
                                                   interactor.Inventory.Remove(item)
                                                   a.Inventory.Add(item)
                                               Next
                                               'loot summary
                                               Dim lines As New List(Of (Text As String, Hue As Integer)) From
                                                   {
                                                       ("You find:", Hues.LightGray)
                                                   }
                                               lines.AddRange(loot.GroupBy(Function(x) x.Descriptor.Name).Select(Function(x) ($"{x.Count} {x.Key}", Hues.LightGray)))
                                               a.Universe.Messages.Add("Salvage!", lines.ToArray)
                                               Dim starSystemGroup = a.Location.Map.YokedGroup(GroupTypes.StarSystem)
                                               starSystemGroup.Statistics(StatisticTypes.Scrap) -= 1
                                               interactor.Recycle()
                                               a.ClearInteractor()
                                           End Sub)
    End Function

    Friend Overrides Function GetText(actor As IActor) As String
        Return "Salvage Scrap"
    End Function
End Class
