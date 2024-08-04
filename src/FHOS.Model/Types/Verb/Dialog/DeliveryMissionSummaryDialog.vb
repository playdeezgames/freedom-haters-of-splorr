Imports FHOS.Data

Friend Class DeliveryMissionSummaryDialog
    Implements IDialog

    Public Sub New(actor As Persistence.IActor, starDock As Persistence.IActor, item As Persistence.IItem)
        Me.Actor = actor
        Me.StarDock = starDock
        Me.Item = item
    End Sub

    Public ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String)) Implements IDialog.Lines
        Get
            Dim result As New List(Of (Hue As Integer, Text As String)) From {
                (Hues.Orange, $"Delivery from {starDock.EntityName}:"),
                (Hues.LightGray, $"Item: {item.EntityName}"),
                (Hues.LightGray, $"Destination: {item.GetDestinationPlanet().EntityName}"),
                (Hues.LightGray, $"Jools Reward: {item.GetJoolsReward()}")
            }
            Return result
        End Get
    End Property

    Public ReadOnly Property Choices As IEnumerable(Of (Text As String, Value As Action)) Implements IDialog.Choices
        Get
            Dim result As New List(Of (Text As String, Value As Action)) From {
                ("Cancel", AddressOf CancelDialog)
            }
            Return result
        End Get
    End Property

    Private Sub CancelDialog()
        actor.Dialog = Nothing
        actor.Yokes.Actor(YokeTypes.Interactor) = starDock
    End Sub

    Private ReadOnly Property actor As Persistence.IActor
    Private ReadOnly Property starDock As Persistence.IActor
    Private ReadOnly Property item As Persistence.IItem
End Class
