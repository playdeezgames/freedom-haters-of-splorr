Imports System.IO
Imports System.Text.Json
Imports FHOS.Data
Imports FHOS.Persistence

Public Class WorldModel
    Implements IWorldModel

    Private worldData As WorldData = Nothing

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
            Return "Average"
        End Get
    End Property

    Public ReadOnly Property GalacticAgeName As String Implements IWorldModel.GalacticAgeName
        Get
            Return "Average"
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
        WorldInitializer.Initialize(World)
    End Sub
End Class
