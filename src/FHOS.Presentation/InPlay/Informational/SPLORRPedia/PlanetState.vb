Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class PlanetState
    Inherits BaseState
    Implements IState

    Private ReadOnly group As IGroupModel

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState, group As IGroupModel)
        MyBase.New(model, ui, endState)
        Me.group = group
    End Sub

    Public Overrides Function Run() As IState
        Return endState
    End Function
End Class
