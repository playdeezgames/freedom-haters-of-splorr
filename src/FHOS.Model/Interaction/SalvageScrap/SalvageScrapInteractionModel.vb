Friend Class SalvageScrapInteractionModel
    Inherits InteractionModel

    Private ReadOnly scrap As Integer

    Public Sub New(actor As Persistence.IActor, scrap As Integer)
        MyBase.New(actor)
        Me.scrap = scrap
    End Sub

    Public Overrides Sub Perform()
        actor.Universe.Messages.Add("Salvage!", ($"You find {scrap} scrap!", Black))
        actor.State.Scrap += scrap
        actor.State.Interactor.Recycle()
        actor.State.Interactor = Nothing
    End Sub
End Class
