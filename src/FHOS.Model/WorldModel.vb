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
    End Sub
End Class
