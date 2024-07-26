Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class ChangeEquipmentItemState
    Inherits BaseState
    Implements IState

    Private ReadOnly equipSlot As IActorEquipmentSlotModel

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState, equipSlot As IActorEquipmentSlotModel)
        MyBase.New(model, ui, endState)
        Me.equipSlot = equipSlot
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        Dim menu As New List(Of (String, IItemModel)) From
            {
                (Choices.Cancel, Nothing)
            }
        menu.AddRange(equipSlot.InstallableItems.Select(Function(x) (x.DisplayName, x)))
        Dim choice = ui.Choose((Mood.Prompt, Prompts.WhichItem), menu.ToArray)
        If choice Is Nothing Then
            Return New ChangeEquipmentState(model, ui, endState)
        End If
        Return New ChangeEquipmentItemConfirmState(model, ui, endState, equipSlot, choice)
    End Function
End Class
