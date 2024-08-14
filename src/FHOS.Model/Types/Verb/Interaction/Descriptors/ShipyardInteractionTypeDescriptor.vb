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
        Return actor.Interactor.Descriptor.Flag(FlagTypes.CanUpgradeShip)
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New InteractionModel(actor, Sub(a)
                                               a.Dialog = New EnterShipyardDialog(a, a.Interactor, Nothing)
                                               a.ClearInteractor()
                                           End Sub)
    End Function
End Class
