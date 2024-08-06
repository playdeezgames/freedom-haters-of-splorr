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

    Public Sub New(actor As IActor, starDock As IActor)
        MyBase.New(actor, starDock)
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
        Dim planetVicinity = StarDock.Yokes.Group(YokeTypes.Planet).Parents.Single(Function(x) x.EntityType = GroupTypes.PlanetVicinity)
        Dim starSystem = planetVicinity.Parents.Single(Function(x) x.EntityType = GroupTypes.StarSystem)
        Dim faction = planetVicinity.Parents.Single(Function(x) x.EntityType = GroupTypes.Faction)
        For Each item In deliveredItems
            Dim jools = item.GetJoolsReward
            Dim reputation = item.GetReputationBonus
            Actor.Yokes.Store(YokeTypes.Wallet).CurrentValue += jools
            Actor.AddReputation(planetVicinity, reputation)
            Actor.AddReputation(starSystem, reputation)
            Actor.AddReputation(faction, reputation)
            Actor.Inventory.Remove(item)
            item.Recycle()
        Next
        actor.Dialog = Nothing
        actor.Yokes.Actor(YokeTypes.Interactor) = starDock
    End Sub
End Class
