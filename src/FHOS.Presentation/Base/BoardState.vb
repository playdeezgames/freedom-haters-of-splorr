Imports FHOS.Model
Imports SPLORR.Presentation

Friend MustInherit Class BoardState
    Inherits BaseState

    Protected Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub
End Class
