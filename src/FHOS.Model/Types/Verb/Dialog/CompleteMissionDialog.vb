Imports FHOS.Data
Imports FHOS.Persistence

Friend Class CompleteMissionDialog
    Inherits BaseDialog

    Private ReadOnly starDock As IActor
    Private ReadOnly Property deliveredItems As IEnumerable(Of IItem)
        Get
            Dim planet = starDock.Yokes.Group(YokeTypes.Planet)
            Return actor.Inventory.Items.Where(Function(x) x.EntityType = ItemTypes.Delivery AndAlso x.GetDestinationPlanet.Id = planet.Id)
        End Get
    End Property

    Public Sub New(actor As IActor, starDock As IActor)
        MyBase.New(actor)
        Me.starDock = starDock
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

    Public Overrides ReadOnly Property Choices As IEnumerable(Of (Text As String, Value As Action))
        Get
            Return New List(Of (Text As String, Value As Action)) From
                {
                    ("Done", AddressOf CloseDialog)
                }
        End Get
    End Property

    Private Sub CloseDialog()
        Dim planet = starDock.Yokes.Group(YokeTypes.Planet)
        For Each item In deliveredItems
            Dim jools = item.GetJoolsReward
            Dim reputation = item.GetReputationBonus
            actor.Yokes.Store(YokeTypes.Wallet).CurrentValue += jools
            actor.SetReputation(planet, If(actor.GetReputation(planet), 0) + reputation)
            'TODO: star system and faction reputation bonus
            actor.Inventory.Remove(item)
            item.Recycle()
        Next
        actor.Dialog = Nothing
        actor.Yokes.Actor(YokeTypes.Interactor) = starDock
    End Sub
End Class
