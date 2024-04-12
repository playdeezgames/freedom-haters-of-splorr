Imports System.IO
Imports System.Text.Json
Imports FHOS.Data
Imports FHOS.Persistence

Public Class WorldModel
    Implements IWorldModel

    Private worldData As WorldData = Nothing
    Private _galacticAge As String = GalacticAges.Average
    Private _galacticDensity As String = GalacticDensities.Average

    Private ReadOnly Property World As IWorld
        Get
            Return New World(worldData)
        End Get
    End Property

    Public ReadOnly Property Board As IBoardModel Implements IWorldModel.Board
        Get
            Return New BoardModel(World)
        End Get
    End Property

    Public ReadOnly Property Avatar As IAvatarModel Implements IWorldModel.Avatar
        Get
            Return New AvatarModel(World)
        End Get
    End Property

    Public ReadOnly Property GalacticDensityName As String Implements IWorldModel.GalacticDensityName
        Get
            Return GalacticDensities.Descriptors(_galacticDensity).DisplayName
        End Get
    End Property

    Public ReadOnly Property GalacticAgeName As String Implements IWorldModel.GalacticAgeName
        Get
            Return GalacticAges.Descriptors(_galacticAge).DisplayName
        End Get
    End Property

    Public ReadOnly Property GalacticDensityOptions As IEnumerable(Of (Text As String, Item As String)) Implements IWorldModel.GalacticDensityOptions
        Get
            Return GalacticDensities.Descriptors.OrderBy(Function(x) x.Value.Ordinal).Select(Function(x) (x.Value.DisplayName, x.Key))
        End Get
    End Property

    Public ReadOnly Property GalacticAgeOptions As IEnumerable(Of (Text As String, Item As String)) Implements IWorldModel.GalacticAgeOptions
        Get
            Return GalacticAges.Descriptors.OrderBy(Function(x) x.Value.Ordinal).Select(Function(x) (x.Value.DisplayName, x.Key))
        End Get
    End Property

    Public Sub Save(filename As String) Implements IWorldModel.Save
        File.WriteAllText(filename, JsonSerializer.Serialize(worldData))
    End Sub

    Public Sub Load(filename As String) Implements IWorldModel.Load
        worldData = JsonSerializer.Deserialize(Of WorldData)(File.ReadAllText(filename))
    End Sub

    Public Sub Abandon() Implements IWorldModel.Abandon
        worldData = Nothing
    End Sub

    Public Sub Embark() Implements IWorldModel.Embark
        worldData = New WorldData()
        WorldInitializer.Initialize(World, _galacticAge, _galacticDensity)
    End Sub

    Public Sub SetGalacticAge(age As String) Implements IWorldModel.SetGalacticAge
        _galacticAge = age
    End Sub

    Public Sub SetGalacticDensity(density As String) Implements IWorldModel.SetGalacticDensity
        _galacticDensity = density
    End Sub
End Class
