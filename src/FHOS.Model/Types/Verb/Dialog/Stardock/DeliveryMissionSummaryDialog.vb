Imports FHOS.Data

Friend Class DeliveryMissionSummaryDialog
    Inherits BaseStarDockMenuDialog

    Public Sub New(
                  actor As Persistence.IActor,
                  starDock As Persistence.IActor,
                  item As Persistence.IItem,
                  finalDialog As IDialog)
        MyBase.New(actor, starDock, finalDialog)
        Me.item = item
    End Sub

    Private ReadOnly Property Deposit As Integer
        Get
            If NeedsDeposit Then
                Return Math.Max(1, item.GetJoolsReward \ 2)
            End If
            Return 0
        End Get
    End Property

    Private ReadOnly Property WorstReputation As Integer
        Get
            Return Actor.GetWorstReputation(StarDock.Yokes.Group(YokeTypes.Planet))
        End Get
    End Property

    Private ReadOnly Property NeedsDeposit As Boolean
        Get
            Return WorstReputation < 0
        End Get
    End Property

    Private ReadOnly Property MaximumCurrentDeliveries As Integer
        Get
            Return If(NeedsDeposit, 0, Math.Max(0, WorstReputation \ 25))
        End Get
    End Property

    Private ReadOnly Property CanAddDelivery As Boolean
        Get
            Return Actor.Inventory.ItemCountOfType(ItemTypes.Delivery) <= MaximumCurrentDeliveries
        End Get
    End Property

    Private Function AcceptMission() As IDialog
        Dim depositAmount = Deposit
        Actor.Yokes.Store(YokeTypes.Wallet).CurrentValue -= depositAmount
        item.SetJoolsReward(Deposit + item.GetJoolsReward)
        StarDock.Inventory.Remove(item)
        Actor.Inventory.Add(item)
        StarDock.GenerateDeliveryMission()
        Return EndDialog()
    End Function

    Private Function CancelDialog() As IDialog
        Actor.Yokes.Actor(YokeTypes.Interactor) = StarDock
        Return EndDialog()
    End Function

    Protected Overrides Function InitializeMenu() As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Dim result As New Dictionary(Of String, Func(Of IDialog)) From {
                {DialogChoices.Cancel, AddressOf CancelDialog}
            }
        If CanAddDelivery AndAlso (Deposit = 0 OrElse Actor.Yokes.Store(YokeTypes.Wallet).CurrentValue > Deposit) Then
            result.Add(DialogChoices.Accept, AddressOf AcceptMission)
        End If
        Return result
    End Function

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Dim result As New List(Of (Hue As Integer, Text As String)) From {
                (Hues.Orange, $"Delivery from {StarDock.EntityName}:"),
                (Hues.LightGray, $"Item: {item.EntityName}"),
                (Hues.LightGray, $"Destination: {item.GetDestinationPlanet().EntityName}"),
                (Hues.LightGray, $"Recipient: {item.GetRecipient()}"),
                (Hues.LightGray, $"Jools Reward: {item.GetJoolsReward()}")
            }
        If Not CanAddDelivery Then
            result.Add((Hues.Yellow, "Based on yer current reputation, you cannot take on more deliveries."))
        End If
        If NeedsDeposit Then
            result.Add((Hues.Yellow, $"Based on yer current reputation, you must pay a deposit of {Deposit}."))
        End If
        Return result
    End Function

    Private ReadOnly item As Persistence.IItem
End Class
