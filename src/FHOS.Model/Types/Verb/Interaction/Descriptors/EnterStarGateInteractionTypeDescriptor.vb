Imports FHOS.Persistence

Friend Class EnterStarGateInteractionTypeDescriptor
    Inherits InteractionTypeDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.EnterStarGate)
    End Sub

    Friend Overrides Function GetText(actor As IActor) As String
        Return "Enter Star Gate"
    End Function

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.Yokes.Actor(YokeTypes.Interactor).Descriptor.Flag(FlagTypes.IsStarGate)
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New InteractionModel(actor, Sub(a)
                                               a.Dialog = New EnterStarGateDialog(a, a.Interactor, Nothing)
                                               a.ClearInteractor()
                                           End Sub)
    End Function
End Class
