﻿Imports FHOS.Model
Imports SPLORR.Presentation

Public Class MainMenuState
    Inherits BaseState
    Implements IState
    Private Sub New(model As IUniverseModel, ui As IUIContext)
        MyBase.New(model, ui, Nothing)
    End Sub
    Public Overrides Function Run() As IState Implements IState.Run
        ui.Clear()
        ui.WriteFiglet((Mood.Title, "Freedom Haters of SPLORR!!"))
        Dim choice = ui.Choose(
                (Mood.Prompt, Prompts.MainMenu),
                Choices.Embark,
                Choices.ScumLoad,
                Choices.Load,
                Choices.Options,
                Choices.About,
                Choices.Quit)
        Select Case choice
            Case Choices.Quit
                If ui.Confirm((Mood.Danger, Confirms.Quit)) Then
                    Return endState
                End If
            Case Choices.Embark
                Return New EmbarkMenuState(model, ui, Me)
            Case Choices.About
                Return New AboutState(model, ui, Me)
            Case Else
                ui.Message((Mood.Warning, $"TODO: {choice}"))
        End Select
        Return Me
    End Function

    Public Shared Sub Start(model As IUniverseModel, ui As IUIContext)
        Dim state As IState = New MainMenuState(model, ui)
        While state IsNot Nothing
            state = state.Run()
        End While
    End Sub
End Class