Friend Class GoodByeInteractionModel
    Inherits InteractionModel

    Public Sub New(actor As Persistence.IActor)
        MyBase.New(actor)
    End Sub

    Public Overrides Sub Perform()
        actor.State.Interactor = Nothing
    End Sub
End Class
