Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class ItemSubstackInspectState
    Inherits BaseState
    Implements IState

    Private ReadOnly substack As IAvatarInventoryItemSubstackModel

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState, substack As IAvatarInventoryItemSubstackModel)
        MyBase.New(model, ui, endState)
        Me.substack = substack
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        ui.WriteLine((Mood.Info, substack.EntityName))
        ui.Message((Mood.Prompt, String.Empty))
        'TODO: stuff to do with the substack
        Return endState
    End Function
End Class
