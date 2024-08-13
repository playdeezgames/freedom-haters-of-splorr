Imports FHOS.Data
Imports FHOS.Persistence

Friend Class SalvageDialog
    Inherits BaseMenuDialog

    Public Sub New(actor As IActor, interactor As IActor, finalDialog As IDialog)
        MyBase.New(actor, finalDialog)
        Me.interactor = interactor
    End Sub

    Protected Overrides Function InitializeMenu() As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Return New Dictionary(Of String, Func(Of IDialog)) From {{DialogChoices.Ok, AddressOf EndDialog}}
    End Function

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Dim result = New List(Of (Hue As Integer, Text As String)) From
                    {
                        (Hues.Orange, "Salvage!"),
                        (Hues.LightGray, "You find:")
                    }
        Dim loot = interactor.Inventory.Items.ToList
        For Each item In loot
            interactor.Inventory.Remove(item)
            Actor.Inventory.Add(item)
        Next
        result.AddRange(loot.GroupBy(Function(x) x.EntityName).Select(Function(x) (Hues.LightGray, $"{x.Count} {x.Key}")))
        Dim starSystemGroup = Actor.Location.Map.YokedGroup(GroupTypes.StarSystem)
        starSystemGroup.Statistics(StatisticTypes.Scrap) -= 1
        interactor.Recycle()
        Return result
    End Function

    Private ReadOnly interactor As IActor
End Class
