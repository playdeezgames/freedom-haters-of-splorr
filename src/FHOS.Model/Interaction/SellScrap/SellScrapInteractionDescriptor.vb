Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class SellScrapInteractionDescriptor
    Inherits InteractionDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.SellScrap, "Sell Scrap")
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.State.Scrap > 0 AndAlso actor.State.Interactor.Properties.BuysScrap
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New SellScrapInteractionModel(actor)
    End Function
End Class
