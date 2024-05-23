Imports SPLORR.UI
Imports FHOS.Model

Friend Class ActionMenuState
    Inherits BasePickerState(Of Model.IUniverseModel, String)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Action Menu",
            context.ChooseOrCancel,
            BoilerplateState.Neutral)
    End Sub

    Private ReadOnly actionMap As IReadOnlyDictionary(Of String, String) =
        New Dictionary(Of String, String) From
        {
            {VerbTypes.DistressSignal, GameState.SignalDistress},
            {VerbTypes.Status, GameState.Status},
            {VerbTypes.SPLORRPedia, GameState.SPLORRPedia},
            {VerbTypes.MoveRight, GameState.MoveRight},
            {VerbTypes.MoveLeft, GameState.MoveLeft},
            {VerbTypes.MoveUp, GameState.MoveUp},
            {VerbTypes.MoveDown, GameState.MoveDown},
            {VerbTypes.Crew, GameState.SelectCrewMember},
            {VerbTypes.Vessel, GameState.SelectVessel}
        }

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        SetState(actionMap(value.Item))
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        Dim result As New List(Of (Text As String, Item As String))
        With Context.Model.State.Avatar
            For Each verb In .Verbs.Available
                result.Add((verb.Text, verb.VerbType))
            Next
        End With
        Return result
    End Function
End Class
