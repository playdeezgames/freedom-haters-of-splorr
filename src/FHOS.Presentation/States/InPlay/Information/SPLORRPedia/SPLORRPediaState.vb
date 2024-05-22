Imports FHOS.Model
Imports SPLORR.UI

Friend Class SPLORRPediaState
    Inherits BasePickerState(Of IUniverseModel, String)
    Private Const FactionListText = "Factions..."
    Private Const StarSystemListText = "Star Systems..."
    Private Const PlanetListText = "Planets..."

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "SPLORR!!Pedia",
            context.ChooseOrCancel,
            GameState.ActionMenu)
    End Sub

    Private ReadOnly actionMap As IReadOnlyDictionary(Of String, String) =
        New Dictionary(Of String, String) From
        {
            {FactionListText, GameState.FactionList},
            {StarSystemListText, GameState.StarSystemList},
            {PlanetListText, GameState.PlanetList}
        }

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        SetState(actionMap(value.Item))
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        Return New List(Of (Text As String, Item As String)) From
            {
                (FactionListText, FactionListText),
                (StarSystemListText, StarSystemListText),
                (PlanetListText, PlanetListText)
            }
    End Function
End Class
