Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class InstallEquipmentState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        With model.State.Avatar.Yokes.Shipyard
            Dim installableSlots = .InstallableEquipmentSlots
            ui.Clear()
            Dim menu As New List(Of (String, IAvatarEquipmentSlotModel)) From
                {
                    (Choices.Cancel, Nothing)
                }
            menu.AddRange(installableSlots.Select(Function(x) (x.SlotName, x)))
            Dim choice = ui.Choose((Mood.Prompt, Prompts.EquipmentSlot), menu.ToArray)
            If choice IsNot Nothing Then
                Return New ChangeEquipmentItemState(model, ui, endState, choice)
            End If
            Return New ShipyardState(model, ui, endState)
        End With
    End Function
End Class
