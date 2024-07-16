Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class PricesState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        Throw New NotImplementedException()
    End Function
End Class
