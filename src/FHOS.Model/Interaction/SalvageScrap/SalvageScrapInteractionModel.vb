Friend Class SalvageScrapInteractionModel
    Implements IInteractionModel

    Private ReadOnly actor As Persistence.IActor
    Private ReadOnly scrap As Integer

    Public Sub New(actor As Persistence.IActor, scrap As Integer)
        Me.actor = actor
        Me.scrap = scrap
    End Sub

    Public Sub Perform() Implements IInteractionModel.Perform
        actor.Universe.Messages.Add("Salvage!", ($"You find {scrap} scrap!", Black))
        actor.State.Scrap += scrap
        actor.State.Interactor.Recycle()
        actor.State.Interactor = Nothing
    End Sub
End Class
