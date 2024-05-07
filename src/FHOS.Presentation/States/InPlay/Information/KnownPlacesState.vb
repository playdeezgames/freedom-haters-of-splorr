Imports FHOS.Model
Imports SPLORR.UI

Friend Class KnownPlacesState
    Inherits BasePickerState(Of IUniverseModel, String)
    Private Const StarSystemListText = "Known Star Systems..."

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Known Places...",
            context.ControlsText(aButton:="Choose", bButton:="Cancel"),
            GameState.ActionMenu)
    End Sub

    Private ReadOnly actionMap As IReadOnlyDictionary(Of String, String) =
        New Dictionary(Of String, String) From
        {
            {GoBackText, BoilerplateState.Neutral},
            {StarSystemListText, GameState.StarSystemList}
        }

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        SetState(actionMap(value.Item))
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        Dim result As New List(Of (Text As String, Item As String)) From
            {
                (GoBackText, GoBackText)
            }
        With Context.Model.Avatar
            If .KnowsPlaces Then
                result.Add((StarSystemListText, StarSystemListText))
            End If
        End With
        Return result
    End Function
End Class
