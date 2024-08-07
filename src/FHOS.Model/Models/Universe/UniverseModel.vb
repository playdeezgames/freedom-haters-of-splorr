﻿Imports System.IO
Imports System.Text.Json
Imports FHOS.Data
Imports FHOS.Persistence

Public Class UniverseModel
    Implements IUniverseModel
    Private ReadOnly WriteStringToFile As Action(Of String, String)
    Private ReadOnly ReadStringFromFile As Func(Of String, String)
    Private ReadOnly initializer As IInitializer
    Private ReadOnly generationTimeSlice As Double

    Private UniverseData As UniverseData = Nothing

    Private ReadOnly Property Universe As IUniverse
        Get
            Return New Universe(UniverseData)
        End Get
    End Property

    Private Sub PersistEmbarkSettings()
        WriteStringToFile(
            EmbarkSettingsFilename,
            JsonSerializer.Serialize(EmbarkSettings))
    End Sub

    Private ReadOnly Property EmbarkSettings As EmbarkSettings
        Get
            If _embarkSettings Is Nothing Then
                Try
                    _embarkSettings = JsonSerializer.Deserialize(Of EmbarkSettings)(ReadStringFromFile(EmbarkSettingsFilename))
                Catch ex As Exception
                    _embarkSettings = New EmbarkSettings
                End Try
            End If
            Return _embarkSettings
        End Get
    End Property

    Public ReadOnly Property Generator As IUniverseGeneratorModel Implements IUniverseModel.Generator
        Get
            Return UniverseGeneratorModel.MakeGenerator(
                Sub() UniverseData = New UniverseData,
                Function() Universe,
                EmbarkSettings,
                initializer,
                generationTimeSlice)
        End Get
    End Property

    Public ReadOnly Property Settings As IUniverseSettingsModel Implements IUniverseModel.Settings
        Get
            Return UniverseSettingsModel.FromSettings(EmbarkSettings, AddressOf PersistEmbarkSettings)
        End Get
    End Property

    Public ReadOnly Property State As IUniverseStateModel Implements IUniverseModel.State
        Get
            Return UniverseStateModel.FromUniverse(Universe)
        End Get
    End Property

    Public ReadOnly Property Pedia As IUniversePediaModel Implements IUniverseModel.Pedia
        Get
            Return UniversePediaModel.FromUniverse(Universe)
        End Get
    End Property

    Public ReadOnly Property SaveState As IUniverseSaveStateModel Implements IUniverseModel.SaveState
        Get
            Return UniverseSaveStateModel.FromUniverse(
                Sub(d) UniverseData = d,
                Function() UniverseData, WriteStringToFile, ReadStringFromFile)
        End Get
    End Property

    Const EmbarkSettingsFilename = "embark-settings.json"
    Private _embarkSettings As EmbarkSettings

    Public Sub New(
                  Optional writeStringToFile As Action(Of String, String) = Nothing,
                  Optional readStringFromFile As Func(Of String, String) = Nothing,
                  Optional initializer As IInitializer = Nothing,
                  Optional generationTimeSlice As Double = 0.01)
        Me.WriteStringToFile = If(writeStringToFile, AddressOf File.WriteAllText)
        Me.ReadStringFromFile = If(readStringFromFile, AddressOf File.ReadAllText)
        Me.initializer = If(initializer, New Initializer)
        Me.generationTimeSlice = generationTimeSlice
    End Sub
End Class
