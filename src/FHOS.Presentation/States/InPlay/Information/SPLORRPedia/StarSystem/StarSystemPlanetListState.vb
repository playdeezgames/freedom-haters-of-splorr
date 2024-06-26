﻿Imports FHOS.Model
Imports SPLORR.UI

Friend Class StarSystemPlanetListState
    Inherits BasePickerState(Of IUniverseModel, IGroupModel)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Planets",
            context.ChooseOrCancel,
            Nothing)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As IGroupModel))
        Context.Model.Ephemerals.SelectedPlanet.Push(value.Item)
        SetState(GameState.PlanetDetails)
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As IGroupModel))
        Return Context.Model.Ephemerals.SelectedStarSystem.Peek.Children.ChildPlanets.Select(Function(x) (x.Name, x)).ToList
    End Function
End Class
