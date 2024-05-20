Imports FHOS.Persistence

Friend Class SellScrapInteractionTypeDescriptor
    Inherits InteractionTypeDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.SellScrap)
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.State.Scrap > 0 AndAlso actor.State.Interactor.Properties.BuysScrap
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New SellScrapInteractionModel(actor)
    End Function

    Friend Overrides Function GetText(actor As IActor) As String
        Return "Sell Scrap"
    End Function
End Class
