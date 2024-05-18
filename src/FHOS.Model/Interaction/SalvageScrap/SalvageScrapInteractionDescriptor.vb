Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class SalvageScrapInteractionDescriptor
    Inherits InteractionDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.SalvageScrap, "Salvage Scrap")
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.State.Interactor.Properties.CanSalvage
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New SalvageScrapInteractionModel(actor, RNG.RollDice("4d6"))
    End Function
End Class
