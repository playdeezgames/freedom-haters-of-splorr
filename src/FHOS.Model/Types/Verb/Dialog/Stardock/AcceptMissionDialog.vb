Imports FHOS.Data
Imports FHOS.Persistence

Friend Class AcceptMissionDialog
    Inherits BaseStarDockDialog

    Public Sub New(
                  actor As IActor,
                  starDock As IActor,
                  finalDialog As IDialog)
        MyBase.New(actor, starDock, finalDialog)
    End Sub

    Public Overrides ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String))
        Get
            Dim result As New List(Of (Hue As Integer, Text As String)) From {
                (Hues.Pink, $"Delivery Missions for {starDock.EntityName}:")
            }
            Return result
        End Get
    End Property

    Public Overrides ReadOnly Property Choices As IEnumerable(Of (Text As String, Value As Action))
        Get
            Dim result As New List(Of (Text As String, Value As Action)) From
                {
                    (DialogChoices.Cancel, AddressOf CancelDialog)
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
        EndDialog()
        Actor.Yokes.Actor(YokeTypes.Interactor) = starDock
    End Sub
    Private Sub AcceptDeliveryMission(item As IItem)
        Actor.Dialog = New DeliveryMissionSummaryDialog(Actor, StarDock, item, finalDialog)
    End Sub
End Class
