Imports FHOS.Persistence

Friend Class SellScrapInteractionTypeDescriptor
    Inherits InteractionTypeDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.SellScrap)
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return If(actor.Statistics(StatisticTypes.Scrap), 0) > 0 AndAlso actor.YokedActor(YokeTypes.Interactor).Descriptor.BuysScrap
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New SellScrapInteractionModel(actor)
    End Function

    Friend Overrides Function GetText(actor As IActor) As String
        Return "Sell Scrap"
    End Function
End Class
