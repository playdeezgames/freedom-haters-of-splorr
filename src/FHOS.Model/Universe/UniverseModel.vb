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
            Return New GalacticAgeModel()
        End Get
    End Property

    Public ReadOnly Property GalacticDensity As IGalacticDensityModel Implements IUniverseModel.GalacticDensity
        Get
            Return New GalacticDensityModel()
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
        UniverseInitializer.Initialize(Universe, GalacticAge.Value, GalacticDensity.Value)
    End Sub
End Class
