Imports FHOS.Model
Imports FHOS.Persistence
Imports SPLORR.UI

Friend Class KnownPlacesState
    Inherits BasePickerState(Of IUniverseModel, String)
    Private Const StarSystemListText = "Known Star Systems..."
    Private Const StarVicinityListText = "Known Star Vicinities..."
    Private Const SatelliteListText = "Known Satellites..."

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Known Places...",
            context.ChooseOrCancel,
            GameState.ActionMenu)
    End Sub

    Private ReadOnly actionMap As IReadOnlyDictionary(Of String, String) =
        New Dictionary(Of String, String) From
        {
            {StarSystemListText, GameState.KnownStarSystemList},
            {StarVicinityListText, GameState.KnownStarVicinityList}
        }

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        SetState(actionMap(value.Item))
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        Dim result As New List(Of (Text As String, Item As String))
        With Context.Model.State.Avatar
            If .KnownPlaces.HasAnyOfType(PlaceTypes.StarSystem) Then
                result.Add((StarSystemListText, StarSystemListText))
            End If
            If .KnownPlaces.HasAnyOfType(PlaceTypes.StarVicinity) Then
                result.Add((StarVicinityListText, StarVicinityListText))
            End If
        End With
        Return result
    End Function
End Class
