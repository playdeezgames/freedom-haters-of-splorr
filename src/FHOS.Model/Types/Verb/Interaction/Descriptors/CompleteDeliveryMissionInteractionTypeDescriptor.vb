Imports FHOS.Persistence

Friend Class CompleteDeliveryMissionInteractionTypeDescriptor
    Inherits InteractionTypeDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.CompleteDeliveryMission)
    End Sub

    Friend Overrides Function GetText(actor As IActor) As String
        Return "Complete Delivery..."
    End Function

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        If Not actor.Inventory.Items.Any(Function(x) x.EntityType = ItemTypes.Delivery) Then
            Return False
        End If
        Dim interactor = actor.Interactor
        If interactor Is Nothing Then
            Return False
        End If
        If interactor.EntityType <> ActorTypes.StarDock Then
            Return False
        End If
        Dim planet = interactor.Yokes.Group(YokeTypes.Planet)
        Return actor.Inventory.Items.Any(Function(x) x.EntityType = ItemTypes.Delivery AndAlso x.GetDestinationPlanet.Id = planet.Id)
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New InteractionModel(actor, Sub(a)
                                               a.Dialog = New CompleteMissionDialog(a, a.Interactor, Nothing)
                                               a.ClearInteractor
                                           End Sub)
    End Function
End Class
