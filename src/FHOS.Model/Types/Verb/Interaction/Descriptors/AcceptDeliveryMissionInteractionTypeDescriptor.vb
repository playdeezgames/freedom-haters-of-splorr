Imports FHOS.Persistence

Friend Class AcceptDeliveryMissionInteractionTypeDescriptor
    Inherits InteractionTypeDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.AcceptDeliveryMission)
    End Sub

    Friend Overrides Function GetText(actor As IActor) As String
        Return "Delivery Mission..."
    End Function

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Dim interactor = actor.Interactor
        If interactor Is Nothing Then
            Return False
        End If
        If interactor.EntityType <> ActorTypes.StarDock Then
            Return False
        End If
        If interactor.Inventory.ItemCountOfType(ItemTypes.Delivery) < 1 Then
            Return False
        End If
        Return True
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New InteractionModel(actor, Sub(a)
                                               a.Dialog = New AcceptMissionDialog(a, a.Interactor)
                                               a.ClearInteractor
                                           End Sub)
    End Function
End Class
