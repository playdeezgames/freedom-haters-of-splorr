﻿Imports FHOS.Data
Imports FHOS.Persistence

Friend Class CompleteMissionDialog
    Inherits BaseInteractorMenuDialog

    Private ReadOnly Property deliveredItems As IEnumerable(Of IItem)
        Get
            Dim planet = interactor.Yokes.Group(YokeTypes.Planet)
            Return Actor.Inventory.Items.Where(Function(x) x.EntityType = ItemTypes.Delivery AndAlso x.GetDestinationPlanet.Id = planet.Id)
        End Get
    End Property

    Public Sub New(actor As IActor, starDock As IActor, finalDialog As IDialog)
        MyBase.New(actor, starDock, finalDialog, String.Empty)
    End Sub

    Private Function CloseDialog() As IDialog
        For Each item In deliveredItems
            Dim jools = item.GetJoolsReward
            Dim reputation = item.GetReputationBonus
            actor.ChangeJools(jools)
            actor.UpdateReputations(
                reputation,
                item.GetOriginPlanet,
                Actor.UpdateReputations(reputation, item.GetDestinationPlanet))
            Actor.Inventory.Remove(item)
            item.Recycle()
        Next
        Actor.Yokes.Actor(YokeTypes.Interactor) = interactor
        Return EndDialog()
    End Function

    Protected Overrides Function InitializeMenu() As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Return New Dictionary(Of String, Func(Of IDialog)) From
                {
                    {DialogChoices.Done, AddressOf CloseDialog}
                }
    End Function

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
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
    End Function
End Class
