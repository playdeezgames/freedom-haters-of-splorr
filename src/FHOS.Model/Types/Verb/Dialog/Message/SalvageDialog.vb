Imports FHOS.Data
Imports FHOS.Persistence

Friend Class SalvageDialog
    Inherits BaseDialog
    Private result As List(Of (Hue As Integer, Text As String)) = Nothing

    Public Sub New(actor As IActor, interactor As IActor, finalDialog As IDialog)
        MyBase.New(DialogType.Menu, actor, finalDialog, Nothing)
        Me.interactor = interactor
    End Sub

    Public Overrides Function Choose(choice As String) As IDialog
        Dim value As Func(Of IDialog) = Nothing
        If LegacyChoices().TryGetValue(choice, value) Then
            Return value()
        End If
        Return Me
    End Function

    Public Overrides ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String))
        Get
            If result Is Nothing Then
                result = New List(Of (Hue As Integer, Text As String)) From
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
            End If
            Return result
        End Get
    End Property

    Private ReadOnly Property LegacyChoices As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Get
            Return New Dictionary(Of String, Func(Of IDialog)) From {{DialogChoices.Ok, AddressOf EndDialog}}
        End Get
    End Property

    Public Overrides ReadOnly Property Menu As IEnumerable(Of String)
        Get
            Return LegacyChoices.Keys
        End Get
    End Property

    Private ReadOnly interactor As IActor
End Class
