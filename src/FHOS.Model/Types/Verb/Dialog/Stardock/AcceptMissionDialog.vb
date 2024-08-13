Imports FHOS.Data
Imports FHOS.Persistence

Friend Class AcceptMissionDialog
    Inherits BaseInteractorMenuDialog

    Public Sub New(
                  actor As IActor,
                  starDock As IActor,
                  finalDialog As IDialog)
        MyBase.New(actor, starDock, finalDialog)
    End Sub

    Private Function ToChoice(item As IItem) As (Text As String, Value As Func(Of IDialog))
        Return (item.EntityName, AcceptDeliveryMission(item))
    End Function

    Private Function CancelDialog() As IDialog
        Actor.Yokes.Actor(YokeTypes.Interactor) = interactor
        Return EndDialog()
    End Function
    Private Function AcceptDeliveryMission(item As IItem) As Func(Of IDialog)
        Return Function() New DeliveryMissionSummaryDialog(Actor, interactor, item, finalDialog)
    End Function

    Protected Overrides Function InitializeMenu() As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Dim result As New List(Of (Text As String, Value As Func(Of IDialog))) From
                {
                    (DialogChoices.Cancel, AddressOf CancelDialog)
                }
        Dim deliveryItems = interactor.Inventory.Items.Where(Function(x) x.EntityType = ItemTypes.Delivery)
        result.AddRange(deliveryItems.Select(AddressOf ToChoice))
        Return result.ToDictionary(Function(x) x.Text, Function(x) x.Value)
    End Function

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Dim result As New List(Of (Hue As Integer, Text As String)) From {
                (Hues.Pink, $"Delivery Missions for {interactor.EntityName}:")
            }
        Return result
    End Function
End Class
