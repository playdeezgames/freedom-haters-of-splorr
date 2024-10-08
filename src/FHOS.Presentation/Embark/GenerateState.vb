﻿Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class GenerateState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        Dim _timeStart = DateTimeOffset.Now
        Do Until model.Generator.Done
            model.Generator.Generate()
            ui.ClearImmediate()
            ui.WriteFigletImmediate((Mood.Title, Messages.Generating))
            ui.WriteLineImmediate((Mood.Info, $"Current Step: {model.Generator.CurrentStep}"))
            ui.WriteLineImmediate((Mood.Info, $"Steps Completed: {model.Generator.StepsCompleted}"))
            ui.WriteLineImmediate((Mood.Info, $"Steps To Go: {model.Generator.StepsRemaining}"))
            ui.WriteLineImmediate((Mood.Info, $"Time Taken: {(DateTimeOffset.Now - _timeStart).TotalSeconds:f1}s"))
        Loop
        ui.Message((Mood.Prompt, String.Empty))
        Return New NeutralState(model, ui, endState)
    End Function
End Class
