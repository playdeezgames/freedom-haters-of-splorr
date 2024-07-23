Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class EquipmentSlotItemState
    Inherits BaseState
    Implements IState

    Private ReadOnly item As IItemModel

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState, item As IItemModel)
        MyBase.New(model, ui, endState)
        Me.item = item
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        ui.WriteLine(
            (Mood.Title, item.DisplayName),
            (Mood.Info, item.Description))
        ui.Message((Mood.Prompt, String.Empty))
        Return New EquipmentState(model, ui, endState)
    End Function
End Class
