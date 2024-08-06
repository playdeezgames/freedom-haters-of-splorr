Imports System.Runtime.Serialization

Friend Class DeliveryMissionSummaryDialog
    Inherits BaseStarDockDialog

    Public Sub New(actor As Persistence.IActor, starDock As Persistence.IActor, item As Persistence.IItem)
        MyBase.New(actor, starDock)
        Me.item = item
    End Sub

    Public Overrides ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String))
        Get
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
        End Get
    End Property

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


    Public Overrides ReadOnly Property Choices As IEnumerable(Of (Text As String, Value As Action))
        Get
            Dim result As New List(Of (Text As String, Value As Action)) From {
                ("Cancel", AddressOf CancelDialog)
            }
            If CanAddDelivery AndAlso (Deposit = 0 OrElse Actor.Yokes.Store(YokeTypes.Wallet).CurrentValue > Deposit) Then
                result.Add(("Accept", AddressOf AcceptMission))
            End If
            Return result
        End Get
    End Property

    Private Sub AcceptMission()
        StarDock.Inventory.Remove(item)
        Actor.Inventory.Add(item)
        StarDock.GenerateDeliveryMission()
        Actor.Dialog = Nothing
    End Sub

    Private Sub CancelDialog()
        Actor.Dialog = Nothing
        Actor.Yokes.Actor(YokeTypes.Interactor) = StarDock
    End Sub
    Private ReadOnly Property item As Persistence.IItem
End Class
