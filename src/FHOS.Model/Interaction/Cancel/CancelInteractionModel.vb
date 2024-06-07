Friend Class CancelInteractionModel
    Inherits InteractionModel

    Public Sub New(actor As Persistence.IActor)
        MyBase.New(actor)
    End Sub

    Public Overrides Sub Perform()
        actor.YokedActor(YokeTypes.Interactor) = Nothing
    End Sub
End Class
