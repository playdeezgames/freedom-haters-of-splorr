Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class UninstallEquipmentState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        With model.State.Avatar.Yokes.Shipyard
            Dim uninstallableSlots = .UninstallableEquipmentSlots
            ui.Clear()
            Dim menu As New List(Of (String, IAvatarEquipmentSlotModel)) From
                {
                    (Choices.Cancel, Nothing)
                }
            menu.AddRange(uninstallableSlots.Select(Function(x) (ToName(x), x)))
            Dim choice = ui.Choose((Mood.Prompt, Prompts.EquipmentSlot), menu.ToArray)
            If choice IsNot Nothing Then
                If choice.UninstallFee > 0 Then
                    If Not ui.Confirm((Mood.Danger, $"The fee is {choice.UninstallFee}. Are you sure you want to continue?")) Then
                        Return Me
                    End If
                End If
                Return New ChangeEquipmentItemCompleteState(model, ui, endState, choice, Nothing)
            End If
            Return New ShipyardState(model, ui, endState)
        End With
    End Function

    Private Function ToName(equipSlot As IAvatarEquipmentSlotModel) As String
        Return equipSlot.SlotName
    End Function
End Class
