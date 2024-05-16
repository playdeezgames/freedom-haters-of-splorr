Imports System.IO
Imports System.Text.Json
Imports FHOS.Data
Imports FHOS.Persistence

Public Class UniverseModel
    Implements IUniverseModel

    Private UniverseData As UniverseData = Nothing

    Private ReadOnly Property Universe As IUniverse
        Get
            Return New Universe(UniverseData)
        End Get
    End Property

    Private Shared Sub PersistEmbarkSettings()
        File.WriteAllText(
            EmbarkSettingsFilename,
            JsonSerializer.Serialize(EmbarkSettings))
    End Sub

    Public Sub Save(filename As String) Implements IUniverseModel.Save
        File.WriteAllText(filename, JsonSerializer.Serialize(UniverseData))
    End Sub

    Public Sub Load(filename As String) Implements IUniverseModel.Load
        UniverseData = JsonSerializer.Deserialize(Of UniverseData)(File.ReadAllText(filename))
    End Sub

    Public Sub Abandon() Implements IUniverseModel.Abandon
        UniverseData = Nothing
    End Sub

    Private Shared ReadOnly Property EmbarkSettings As EmbarkSettings
        Get
            If _embarkSettings Is Nothing Then
                Try
                    _embarkSettings = JsonSerializer.Deserialize(Of EmbarkSettings)(File.ReadAllText(EmbarkSettingsFilename))
                Catch ex As Exception
                    _embarkSettings = New EmbarkSettings
                End Try
            End If
            Return _embarkSettings
        End Get
    End Property

    Public ReadOnly Property FactionList As IEnumerable(Of (Text As String, Faction As IFactionModel)) Implements IUniverseModel.FactionList
        Get
            Return Universe.Factions.Select(Function(x) (x.Name, FactionModel.FromFaction(x)))
        End Get
    End Property

    Public ReadOnly Property StarSystems As IEnumerable(Of (Text As String, Place As IPlaceModel)) Implements IUniverseModel.StarSystems
        Get
            Return Universe.
                GetPlacesOfType(PlaceTypes.StarSystem).
                OrderBy(Function(x) x.Name).
                Select(Function(x) (x.Name, PlaceModel.FromPlace(x)))
        End Get
    End Property

    Public ReadOnly Property Planets As IEnumerable(Of (Text As String, Place As IPlaceModel)) Implements IUniverseModel.Planets
        Get
            Return Universe.
                GetPlacesOfType(PlaceTypes.Planet).
                OrderBy(Function(x) x.Name).
                Select(Function(x) (x.Name, PlaceModel.FromPlace(x)))
        End Get
    End Property

    Public ReadOnly Property Satellites As IEnumerable(Of (Text As String, Place As IPlaceModel)) Implements IUniverseModel.Satellites
        Get
            Return Universe.
                GetPlacesOfType(PlaceTypes.Satellite).
                OrderBy(Function(x) x.Name).
                Select(Function(x) (x.Name, PlaceModel.FromPlace(x)))
        End Get
    End Property

    Public ReadOnly Property Generator As IUniverseGeneratorModel Implements IUniverseModel.Generator
        Get
            Return UniverseGeneratorModel.MakeGenerator(
                Sub() UniverseData = New UniverseData,
                Function() Universe,
                EmbarkSettings)
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

    Const EmbarkSettingsFilename = "embark-settings.json"
    Private Shared _embarkSettings As EmbarkSettings
End Class
