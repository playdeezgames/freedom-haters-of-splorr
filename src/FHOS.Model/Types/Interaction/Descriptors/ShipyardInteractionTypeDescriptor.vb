Imports FHOS.Persistence

Friend Class ShipyardInteractionTypeDescriptor
    Inherits InteractionTypeDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.EnterShipyard)
    End Sub

    Friend Overrides Function GetText(actor As IActor) As String
        Return "Enter Shipyard..."
    End Function

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.Interactor.Descriptor.CanUpgradeShip
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New InteractionModel(actor, Sub(a)
                                               a.Yokes.Actor(YokeTypes.Shipyard) = a.Interactor
                                               a.ClearInteractor()
                                           End Sub)
    End Function
End Class
