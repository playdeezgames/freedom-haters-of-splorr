Friend Class DeliveryMissionSummaryDialog
    Inherits BaseStardockDialog

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
            Return result
        End Get
    End Property

    Public Overrides ReadOnly Property Choices As IEnumerable(Of (Text As String, Value As Action))
        Get
            Dim result As New List(Of (Text As String, Value As Action)) From {
                ("Cancel", AddressOf CancelDialog),
                ("Accept", AddressOf AcceptMission)
            }
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
