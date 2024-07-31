Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class StatusState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        Dim avatar = model.State.Avatar
        ui.WriteLine(
            (Mood.Title, Messages.Status),
            (moodTable(Hues.ForPercentage(avatar.Vessel.OxygenPercent)), $"O2: ({avatar.Vessel.OxygenQuantity}/{avatar.Vessel.OxygenMaximum}){avatar.Vessel.OxygenPercent}%"))
        Dim fuel = avatar.Vessel.FuelPercent
        If Fuel.HasValue Then
            ui.WriteLine((moodTable(Hues.ForPercentage(fuel.Value)), $"Fuel: ({avatar.Vessel.FuelQuantity}/{avatar.Vessel.FuelMaximum}){fuel.Value}%"))
        End If
        ui.WriteLine(
        (Mood.Info, $"Faction: {avatar.Bio.Faction.Name}"),
        (Mood.Info, $"Home Planet: {avatar.Bio.HomePlanet.Name}"))
        ui.Message((Mood.Prompt, String.Empty))
        Return New NeutralState(model, ui, endState)
    End Function
End Class
