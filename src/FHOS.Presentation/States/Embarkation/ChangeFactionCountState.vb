Imports FHOS.Model
Imports SPLORR.UI

Friend Class ChangeFactionCountState
    Inherits BasePickerState(Of IUniverseModel, Integer)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Faction Count",
            context.ChooseOrCancel,
            BoilerplateState.Embark)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As Integer))
        Context.Model.Settings.FactionCount.SetFactionCount(value.Item)
        SetState(BoilerplateState.Embark)
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As Integer))
        Dim result As New List(Of (Text As String, Item As Integer))
        result.AddRange(
            Context.Model.Settings.FactionCount.Options.Select(
                Function(x) (If(x.Item = Context.Model.Settings.FactionCount.Current, $"** {x.Text} **", x.Text), x.Item)))
        Return result
    End Function
End Class
