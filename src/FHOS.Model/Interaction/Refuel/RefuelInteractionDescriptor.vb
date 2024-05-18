Imports FHOS.Persistence

Friend Class RefuelInteractionDescriptor
    Inherits InteractionDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.Refuel, "Refuel")
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.State.Interactor.Properties.CanRefuel
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New RefuelInteractionModel(actor)
    End Function
End Class
