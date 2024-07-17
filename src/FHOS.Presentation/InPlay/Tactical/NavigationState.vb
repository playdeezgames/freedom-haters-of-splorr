Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class NavigationState
    Inherits BoardState
    Implements IState
    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        ui.Write((Mood.Prompt, "Arrows: "))
        ui.Write((Mood.Info, "Move | "))
        ui.Write((Mood.Prompt, "Space/Enter: "))
        ui.Write((Mood.Info, "Actions... | "))
        ui.Write((Mood.Prompt, "Tab: "))
        ui.Write((Mood.Info, "Scanner | "))
        ui.Write((Mood.Prompt, "Escape: "))
        ui.WriteLine((Mood.Info, "Game Menu"))
        RenderBoard()
        With model.State.Avatar
            ui.WriteLine((Mood.Purple, $"NAV SCREEN"))
            ui.WriteLine((Mood.Info, $"{ .State.MapName} ({ .State.Position.X},{ .State.Position.Y})"))
            ui.WriteLine((Mood.Info, $"Turn: { model.State.Turn}"))
            ui.WriteLine((Mood.Info, $"Jools: { .Jools}"))
            ui.WriteLine((moodTable(Hues.ForPercentage(.Vessel.OxygenPercent)), $"O2: { .Vessel.OxygenPercent}%"))
            Dim fuel = .Vessel.FuelPercent
            If fuel.HasValue Then
                ui.WriteLine((moodTable(Hues.ForPercentage(fuel.Value)), $"Fuel: { fuel.Value}%"))
            End If
        End With
        Dim key = ui.ReadKey()
        Select Case key
            Case KeyNames.Escape
                Return New GameMenu(model, ui, Me, endState)
            Case KeyNames.UpArrow
                Return New MoveUpState(model, ui, endState)
            Case KeyNames.RightArrow
                Return New MoveRightState(model, ui, endState)
            Case KeyNames.DownArrow
                Return New MoveDownState(model, ui, endState)
            Case KeyNames.LeftArrow
                Return New MoveLeftState(model, ui, endState)
            Case KeyNames.Spacebar, KeyNames.Enter
                Return New ActionMenuState(model, ui, endState)
            Case KeyNames.Tab
                Return New ScannerState(model, ui, endState, (0, 0))
            Case Else
                Return New NeutralState(model, ui, endState)
        End Select
    End Function
End Class
