﻿Imports FHOS.Model
Imports SPLORR.UI

Friend Class PlanetSatelliteListState
    Inherits BasePickerState(Of IUniverseModel, IGroupModel)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Satellites",
            context.ChooseOrCancel,
            Nothing)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As IGroupModel))
        SatelliteListState.SelectedSatellite.Push(value.Item)
        SetState(GameState.SatelliteDetails)
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As IGroupModel))
        Return PlanetListState.SelectedPlanet.Peek.SatelliteList.Select(Function(x) (x.Name, x)).ToList
    End Function
End Class