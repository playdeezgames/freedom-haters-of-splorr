﻿Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class SalvageScrapInteractionTypeDescriptor
    Inherits InteractionTypeDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.SalvageScrap)
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.Interactor.Descriptor.CanSalvage
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New SalvageScrapInteractionModel(actor)
    End Function

    Friend Overrides Function GetText(actor As IActor) As String
        Return "Salvage Scrap"
    End Function
End Class
