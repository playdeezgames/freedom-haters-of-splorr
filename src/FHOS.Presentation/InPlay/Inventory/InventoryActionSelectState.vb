Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class InventoryActionSelectState
    Inherits BaseState
    Implements IState

    Private ReadOnly itemStack As IAvatarInventoryItemStackModel

    Public Sub New(
                  model As IUniverseModel,
                  ui As IUIContext,
                  endState As IState,
                  itemStack As IAvatarInventoryItemStackModel)
        MyBase.New(model, ui, endState)
        Me.itemStack = itemStack
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        Dim result As New List(Of (Text As String, Item As String)) From {
                (Choices.Leave, Choices.Leave),
                (Choices.Inspect, Choices.Inspect)
            }
        If itemStack.CanUse Then
            result.Add((Choices.Use, Choices.Use))
        End If
        Dim choice = ui.Choose((Mood.Prompt, $"{itemStack.ItemName}(x{itemStack.Count})"), result.ToArray)
        Select Case choice
            Case Choices.Use
                itemStack.Use()
                Return New NeutralState(model, ui, endState)
            Case Choices.Inspect
                Return New InventoryInspectState(model, ui, endState, itemStack)
            Case Else
                Return New InventoryState(model, ui, endState)
        End Select
    End Function
End Class
