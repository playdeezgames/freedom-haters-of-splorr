Imports System.IO
Imports System.Text.Json
Imports FHOS.Data
Imports FHOS.Persistence

Public Class UniverseModel
    Implements IUniverseModel

    Private universeData As UniverseData = Nothing

    Private ReadOnly Property Universe As IUniverse
        Get
            Return New Universe(universeData)
        End Get
    End Property

    Public ReadOnly Property Board As IBoardModel Implements IUniverseModel.Board
        Get
            Return New BoardModel(Universe)
        End Get
    End Property

    Public ReadOnly Property Avatar As IAvatarModel Implements IUniverseModel.Avatar
        Get
            Return New AvatarModel(Universe.Avatar)
        End Get
    End Property

    Public ReadOnly Property GalacticAge As IGalacticAgeModel Implements IUniverseModel.GalacticAge
        Get
            Return New GalacticAgeModel(EmbarkSettings, AddressOf PersistEmbarkSettings)
        End Get
    End Property

    Private Shared Sub PersistEmbarkSettings()
        File.WriteAllText(
            EmbarkSettingsFilename,
            JsonSerializer.Serialize(EmbarkSettings))
    End Sub

    Public ReadOnly Property GalacticDensity As IGalacticDensityModel Implements IUniverseModel.GalacticDensity
        Get
            Return New GalacticDensityModel(EmbarkSettings, AddressOf PersistEmbarkSettings)
        End Get
    End Property

    Public ReadOnly Property Messages As IMessagesModel Implements IUniverseModel.Messages
        Get
            Return New MessagesModel(Universe)
        End Get
    End Property

    Public Sub Save(filename As String) Implements IUniverseModel.Save
        File.WriteAllText(filename, JsonSerializer.Serialize(universeData))
    End Sub

    Public Sub Load(filename As String) Implements IUniverseModel.Load
        universeData = JsonSerializer.Deserialize(Of UniverseData)(File.ReadAllText(filename))
    End Sub

    Public Sub Abandon() Implements IUniverseModel.Abandon
        universeData = Nothing
    End Sub

    Public Sub Embark() Implements IUniverseModel.Embark
        universeData = New UniverseData()
        Initializer.Start(Universe, EmbarkSettings)
    End Sub

    Public Sub Generate() Implements IUniverseModel.Generate
        Dim endTime = DateTimeOffset.Now.AddSeconds(0.01)
        Do
            Initializer.Execute()
        Loop Until DateTimeOffset.Now >= endTime
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

    Public ReadOnly Property StartingWealth As IStartingWealthLevelModel Implements IUniverseModel.StartingWealth
        Get
            Return New StartingWealthLevelModel(EmbarkSettings, AddressOf PersistEmbarkSettings)
        End Get
    End Property

    Public ReadOnly Property GenerationStepsRemaining As Integer Implements IUniverseModel.GenerationStepsRemaining
        Get
            Return Initializer.StepsRemaining
        End Get
    End Property

    Public ReadOnly Property GenerationStepsCompleted As Integer Implements IUniverseModel.GenerationStepsCompleted
        Get
            Return Initializer.StepsDone
        End Get
    End Property

    Public ReadOnly Property DoneGenerating As Boolean Implements IUniverseModel.DoneGenerating
        Get
            Return Initializer.StepsRemaining = 0
        End Get
    End Property

    Public ReadOnly Property FactionCount As IFactionCountModel Implements IUniverseModel.FactionCount
        Get
            Return New FactionCountModel(EmbarkSettings, AddressOf PersistEmbarkSettings)
        End Get
    End Property

    Public ReadOnly Property FactionList As IEnumerable(Of (String, IFactionModel)) Implements IUniverseModel.FactionList
        Get
            Return Universe.Factions.Select(Function(x) (x.Name, CType(New FactionModel(x), IFactionModel)))
        End Get
    End Property

    Public ReadOnly Property StarSystems As IEnumerable(Of (Text As String, Place As IPlaceModel)) Implements IUniverseModel.StarSystems
        Get
            Return Universe.
                GetPlacesOfType(PlaceTypes.StarSystem).
                OrderBy(Function(x) x.Name).
                Select(Function(x) (x.Name, CType(New PlaceModel(x), IPlaceModel)))
        End Get
    End Property

    Public ReadOnly Property Planets As IEnumerable(Of (Text As String, Place As IPlaceModel)) Implements IUniverseModel.Planets
        Get
            Return Universe.
                GetPlacesOfType(PlaceTypes.Planet).
                OrderBy(Function(x) x.Name).
                Select(Function(x) (x.Name, CType(New PlaceModel(x), IPlaceModel)))
        End Get
    End Property

    Public ReadOnly Property Satellites As IEnumerable(Of (Text As String, Place As IPlaceModel)) Implements IUniverseModel.Satellites
        Get
            Return Universe.
                GetPlacesOfType(PlaceTypes.Satellite).
                OrderBy(Function(x) x.Name).
                Select(Function(x) (x.Name, CType(New PlaceModel(x), IPlaceModel)))
        End Get
    End Property

    Const EmbarkSettingsFilename = "embark-settings.json"
    Private Shared _embarkSettings As EmbarkSettings
End Class
