Imports System.IO
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

    Public Sub Save(filename As String) Implements IUniverseModel.Save
        WriteStringToFile(filename, JsonSerializer.Serialize(UniverseData))
    End Sub

    Public Sub Load(filename As String) Implements IUniverseModel.Load
        UniverseData = JsonSerializer.Deserialize(Of UniverseData)(ReadStringFromFile(filename))
        ClearEphemerals()
    End Sub

    Public Sub Abandon() Implements IUniverseModel.Abandon
        UniverseData = Nothing
        ClearEphemerals()
    End Sub

    Private Sub ClearEphemerals()
        SelectedFaction.Clear()
        SelectedPlanet.Clear()
        SelectedSatellite.Clear()
        SelectedStarSystem.Clear()
        CurrentOffer = Nothing
        CurrentPrice = Nothing
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

    Public ReadOnly Property SelectedFaction As Stack(Of IGroupModel) Implements IUniverseModel.SelectedFaction

    Public ReadOnly Property SelectedPlanet As Stack(Of IGroupModel) Implements IUniverseModel.SelectedPlanet

    Public ReadOnly Property SelectedSatellite As Stack(Of IGroupModel) Implements IUniverseModel.SelectedSatellite

    Public ReadOnly Property SelectedStarSystem As Stack(Of IGroupModel) Implements IUniverseModel.SelectedStarSystem

    Public Property CurrentOffer As IAvatarTraderOfferModel Implements IUniverseModel.CurrentOffer

    Public Property CurrentPrice As IAvatarTraderPriceModel Implements IUniverseModel.CurrentPrice

    Public ReadOnly Property Ephemerals As IUniverseEphemeralsModel Implements IUniverseModel.Ephemerals

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
        Me.SelectedFaction = New Stack(Of IGroupModel)
        Me.SelectedPlanet = New Stack(Of IGroupModel)
        Me.SelectedSatellite = New Stack(Of IGroupModel)
        Me.SelectedStarSystem = New Stack(Of IGroupModel)
        Me.Ephemerals = New UniverseEphemeralsModel
    End Sub
End Class
