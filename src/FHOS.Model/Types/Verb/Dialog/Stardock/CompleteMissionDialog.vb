Imports FHOS.Data
Imports FHOS.Persistence

Friend Class CompleteMissionDialog
    Inherits BaseStardockDialog

    Private ReadOnly Property deliveredItems As IEnumerable(Of IItem)
        Get
            Dim planet = StarDock.Yokes.Group(YokeTypes.Planet)
            Return Actor.Inventory.Items.Where(Function(x) x.EntityType = ItemTypes.Delivery AndAlso x.GetDestinationPlanet.Id = planet.Id)
        End Get
    End Property

    Public Sub New(actor As IActor, starDock As IActor, finalDialog As IDialog)
        MyBase.New(actor, starDock, finalDialog)
    End Sub

    Public Overrides ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String))
        Get
            Dim result As New List(Of (Hue As Integer, Text As String)) From
                {
                    (Hues.Orange, "Delivery Complete!")
                }
            Dim totalJools As Integer = 0
            Dim totalReputation As Integer = 0
            For Each item In deliveredItems
                Dim jools = item.GetJoolsReward
                Dim reputation = item.GetReputationBonus
                result.Add((Hues.LightGray, $"{item.EntityName}(Jools: +{jools}, Reputation: +{reputation}"))
                totalJools += jools
                totalReputation += reputation
            Next
            result.Add((Hues.LightGray, $"Total Jools: +{totalJools}"))
            result.Add((Hues.LightGray, $"Total Reputation: +{totalReputation}"))
            Return result
        End Get
    End Property

    Public Overrides ReadOnly Property Choices As IEnumerable(Of (Text As String, Value As Func(Of IDialog)))
        Get
            Return New List(Of (Text As String, Value As Func(Of IDialog))) From
                {
                    (DialogChoices.Done, AddressOf CloseDialog)
                }
        End Get
    End Property

    Private Function CloseDialog() As IDialog
        For Each item In deliveredItems
            Dim jools = item.GetJoolsReward
            Dim reputation = item.GetReputationBonus
            Actor.Yokes.Store(YokeTypes.Wallet).CurrentValue += jools
            Actor.UpdateReputations(
                reputation,
                item.GetOriginPlanet,
                Actor.UpdateReputations(reputation, item.GetDestinationPlanet))
            Actor.Inventory.Remove(item)
            item.Recycle()
        Next
        Actor.Yokes.Actor(YokeTypes.Interactor) = StarDock
        Return EndDialog()
    End Function
End Class
