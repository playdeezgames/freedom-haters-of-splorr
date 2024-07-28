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
        menu.AddRange(equipSlot.InstallableItems.Select(Function(x) (ToName(x), x)))
        Dim choice = ui.Choose((Mood.Prompt, Prompts.WhichItem), menu.ToArray)
        If choice Is Nothing Then
            If equipSlot.HasItem Then
                Return New ChangeEquipmentState(model, ui, endState)
            End If
            Return New InstallEquipmentState(model, ui, endState)
        End If
        Dim fee = ToFee(choice)
        If fee > 0 AndAlso model.State.Avatar.Jools < fee Then
            ui.Message((Mood.Danger, Messages.InsufficientFunds))
            Return Me
        End If
        Return New ChangeEquipmentItemCompleteState(model, ui, endState, equipSlot, choice)
    End Function

    Private Function ToName(itemModel As IItemModel) As String
        Dim changeFee = ToFee(itemModel)
        Return $"{itemModel.DisplayName}{If(changeFee > 0, $"(fee: {changeFee})", String.Empty)}"
    End Function

    Private Function ToFee(itemModel As IItemModel) As Integer
        Return equipSlot.UninstallFee + itemModel.InstallFee
    End Function
End Class
