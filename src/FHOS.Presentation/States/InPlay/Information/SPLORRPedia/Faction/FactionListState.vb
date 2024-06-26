﻿Imports FHOS.Model
Imports SPLORR.UI

Friend Class FactionListState
    Inherits BasePickerState(Of Model.IUniverseModel, IGroupModel)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Galactic Factions",
            context.ChooseOrCancel,
            Nothing)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As IGroupModel))
        Context.Model.Ephemerals.SelectedFaction.Push(value.Item)
        PushState(GameState.FactionDetails)
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As IGroupModel))
        Return Context.Model.Pedia.Factions.Select(Function(x) (x.Name, x)).ToList
    End Function
End Class
