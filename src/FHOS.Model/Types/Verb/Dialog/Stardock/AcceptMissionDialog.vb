﻿Imports FHOS.Data
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

    Public Overrides ReadOnly Property Choices As IEnumerable(Of (Text As String, Value As Func(Of IDialog)))
        Get
            Dim result As New List(Of (Text As String, Value As Func(Of IDialog))) From
                {
                    (DialogChoices.Cancel, AddressOf CancelDialog)
                }
            Dim deliveryItems = StarDock.Inventory.Items.Where(Function(x) x.EntityType = ItemTypes.Delivery)
            result.AddRange(deliveryItems.Select(AddressOf ToChoice))
            Return result
        End Get
    End Property

    Private Function ToChoice(item As IItem) As (Text As String, Value As Func(Of IDialog))
        Return (item.EntityName, AcceptDeliveryMission(item))
    End Function

    Private Function CancelDialog() As IDialog
        Actor.Yokes.Actor(YokeTypes.Interactor) = StarDock
        Return EndDialog()
    End Function
    Private Function AcceptDeliveryMission(item As IItem) As Func(Of IDialog)
        Return Function() New DeliveryMissionSummaryDialog(Actor, StarDock, item, finalDialog)
    End Function
End Class
