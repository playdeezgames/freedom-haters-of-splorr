Friend Class CancelInteractionModel
    Inherits InteractionModel

    Public Sub New(actor As Persistence.IActor)
        MyBase.New(actor, Sub(a) a.ClearInteractor)
    End Sub
End Class
