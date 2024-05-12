Imports FHOS.Model
Imports SPLORR.UI

Friend Class FactionListState
    Inherits BasePickerState(Of Model.IUniverseModel, IFactionModel)
    Friend Shared SelectedFaction As IFactionModel

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Galactic Factions",
            context.ControlsText(aButton:="Choose", bButton:="Cancel"),
            GameState.SPLORRPedia)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As IFactionModel))
        SelectedFaction = value.Item
        SetState(GameState.FactionDetails)
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As IFactionModel))
        Return Context.Model.FactionList.ToList
    End Function
End Class
