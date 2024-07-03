Imports SPLORR.UI

Friend Class InventoryActionSelectState
    Inherits BasePickerState(Of Model.IUniverseModel, String)
    Const InspectText = "Inspect..."
    Const UseText = "Use"

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "<placeholder>",
            context.ChooseOrCancel,
            GameState.Inventory)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        Select Case value.Item
            Case InspectText
                SetState(GameState.InventoryInspect)
            Case UseText
                SetState(GameState.UseItem)
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        With Context.Model.Ephemerals.InventoryItemStack
            HeaderText = .ItemName
            Dim result As New List(Of (Text As String, Item As String)) From {
                (InspectText, InspectText)
            }
            If .CanUse Then
                result.Add((UseText, UseText))
            End If
            Return result
        End With
    End Function
End Class
