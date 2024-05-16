Imports FHOS.Model
Imports SPLORR.UI

Friend Class EmbarkState
    Inherits BasePickerState(Of IUniverseModel, String)
    Private Const AgeText = "age"
    Private Const DensityText = "density"
    Private Const WealthText = "wealth"
    Private Const FactionCountText = "faction-count"
    Private Const GoText = "go"

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Embarkation Options...",
            context.ChooseOrCancel,
            BoilerplateState.MainMenu)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        Select Case value.Item
            Case GoText
                Context.Model.Generator.Start()
                SetState(BoilerplateState.Neutral)
            Case AgeText
                SetState(GameState.ChangeGalacticAge)
            Case DensityText
                SetState(GameState.ChangeGalacticDensity)
            Case WealthText
                SetState(GameState.ChangeStartingWealthLevel)
            Case FactionCountText
                SetState(GameState.ChangeFactionCount)
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        Return New List(Of (Text As String, Item As String)) From
            {
                ("Go!", GoText),
                ($"Change Galactic Age...", AgeText),
                ($"Change Galactic Density...", DensityText),
                ($"Change Starting Wealth...", WealthText),
                ($"Change Faction Count...", FactionCountText)
            }
    End Function

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        MyBase.Render(displayBuffer)
        Dim uiFont = Context.Font(UIFontName)
        uiFont.WriteCenteredText(displayBuffer, (Context.ViewCenter.X, uiFont.Height), $"Galactic Age: {Context.Model.Settings.GalacticAge.CurrentName}", Black)
        uiFont.WriteCenteredText(displayBuffer, (Context.ViewCenter.X, uiFont.Height * 2), $"Galactic Density: {Context.Model.Settings.GalacticDensity.CurrentName}", Black)
        uiFont.WriteCenteredText(displayBuffer, (Context.ViewCenter.X, uiFont.Height * 3), $"Starting Wealth: {Context.Model.Settings.StartingWealth.CurrentName}", Black)
        uiFont.WriteCenteredText(displayBuffer, (Context.ViewCenter.X, uiFont.Height * 4), $"Faction Count: {Context.Model.Settings.FactionCount.CurrentName}", Black)
    End Sub
End Class
