Imports FHOS.Model
Imports SPLORR.UI

Friend Class EmbarkState
    Inherits BasePickerState(Of IWorldModel, String)
    Private Const AgeText = "age"
    Private Const DensityText = "density"
    Private Const GoText = "go"

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of IWorldModel))
        MyBase.New(parent, setState, context, "Embarkation Options...", context.ControlsText("Choose", "Cancel"), BoilerplateState.MainMenu)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        Select Case value.Item
            Case GoText
                Context.Model.Embark()
                SetState(BoilerplateState.Neutral)
            Case AgeText
                SetState(GameState.ChangeGalacticAge)
            Case DensityText
                SetState(GameState.ChangeGalacticDensity)
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        Return New List(Of (Text As String, Item As String)) From
            {
                ("Go!", GoText),
                ($"Change Galactic Age...", AgeText),
                ($"Change Galactic Density...", DensityText)
            }
    End Function

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        MyBase.Render(displayBuffer)
        Dim uiFont = Context.Font(UIFontName)
        uiFont.WriteLeftText(displayBuffer, (0, uiFont.Height), $"Galactic Age: {Context.Model.GalacticAgeName}", Black)
        uiFont.WriteLeftText(displayBuffer, (0, uiFont.Height * 2), $"Galactic Density: {Context.Model.GalacticDensityName}", Black)
    End Sub
End Class
