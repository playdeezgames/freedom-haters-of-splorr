﻿Imports FHOS.Model
Imports SPLORR.UI

Friend Class StarSystemListState
    Inherits BasePickerState(Of IUniverseModel, IPlaceModel)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Star Systems",
            context.ControlsText(bButton:="Cancel"),
            GameState.SPLORRPedia)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As IPlaceModel))
        Throw New NotImplementedException()
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As IPlaceModel))
        Return Context.Model.StarSystems.ToList
    End Function
End Class