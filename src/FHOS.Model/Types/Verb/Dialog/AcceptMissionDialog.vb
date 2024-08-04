Imports FHOS.Data
Imports FHOS.Persistence

Friend Class AcceptMissionDialog
    Implements IDialog

    Private ReadOnly actor As IActor
    Private ReadOnly starDock As IActor

    Public Sub New(actor As IActor, starDock As IActor)
        Me.actor = actor
        Me.starDock = starDock
    End Sub

    Public ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String)) Implements IDialog.Lines
        Get
            Dim result As New List(Of (Hue As Integer, Text As String)) From {
                (Hues.Pink, $"Delivery Missions for {starDock.EntityName}:")
            }
            Return result
        End Get
    End Property

    Public ReadOnly Property Choices As IEnumerable(Of (Text As String, Value As Action)) Implements IDialog.Choices
        Get
            Dim result As New List(Of (Text As String, Value As Action)) From
                {
                    ("Cancel", AddressOf CancelDialog)
                }
            Dim deliveryItems = starDock.Inventory.Items.Where(Function(x) x.EntityType = ItemTypes.Delivery)
            result.AddRange(deliveryItems.Select(AddressOf ToChoice))
            Return result
        End Get
    End Property

    Private Function ToChoice(item As IItem) As (Text As String, Value As Action)
        Return (item.EntityName, Sub() AcceptDeliveryMission(item))
    End Function

    Private Sub CancelDialog()
        actor.Dialog = Nothing
        actor.Yokes.Actor(YokeTypes.Interactor) = starDock
    End Sub
    Private Sub AcceptDeliveryMission(item As IItem)
        actor.Dialog = New DeliveryMissionSummaryDialog(actor, starDock, item)
    End Sub
End Class
