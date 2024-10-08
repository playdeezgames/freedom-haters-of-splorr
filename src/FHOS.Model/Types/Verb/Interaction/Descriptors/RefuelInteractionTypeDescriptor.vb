﻿Imports FHOS.Persistence

Friend Class RefuelInteractionTypeDescriptor
    Inherits InteractionTypeDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.Refuel)
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.Interactor.Descriptor.Flag(FlagTypes.CanRefuel)
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New InteractionModel(actor, Sub(a)
                                               a.Dialog = New RefueledDialog(a, a.Dialog)
                                           End Sub)
    End Function

    Friend Overrides Function GetText(actor As IActor) As String
        'TODO: give me the price!
        Return "Refuel"
    End Function
End Class
