Imports FHOS.Persistence

Friend Class EnterWormholeInteractionTypeDescriptor
    Inherits InteractionTypeDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.EnterWormhole)
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.State.Interactor.Properties.OtherWormhole IsNot Nothing
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New EnterWormholeInteractionModel(actor)
    End Function

    Friend Overrides Function GetText(actor As IActor) As String
        Return "Enter Wormhole"
    End Function
End Class
