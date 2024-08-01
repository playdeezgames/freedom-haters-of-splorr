Imports System.IO
Imports System.Text.Json
Imports FHOS.Data
Imports FHOS.Persistence

Public Class UniverseSaveStateModel
    Implements IUniverseSaveStateModel

    Private ReadOnly WriteStringToFile As Action(Of String, String)
    Private ReadOnly ReadStringFromFile As Func(Of String, String)
    Private ReadOnly setter As Action(Of UniverseData)
    Private ReadOnly getter As Func(Of UniverseData)

    Public Sub New(
                  setter As Action(Of UniverseData),
                  getter As Func(Of UniverseData),
                  writeStringToFile As Action(Of String, String),
                  readStringFromFile As Func(Of String, String))
        Me.WriteStringToFile = writeStringToFile
        Me.ReadStringFromFile = readStringFromFile
        Me.setter = setter
        Me.getter = getter
    End Sub

    Public Sub Save(filename As String) Implements IUniverseSaveStateModel.Save
        WriteStringToFile(filename, JsonSerializer.Serialize(getter()))
    End Sub

    Public Sub Load(filename As String) Implements IUniverseSaveStateModel.Load
        setter(JsonSerializer.Deserialize(Of UniverseData)(ReadStringFromFile(filename)))
    End Sub

    Public Sub Abandon() Implements IUniverseSaveStateModel.Abandon
        setter(Nothing)
    End Sub

    Friend Shared Function FromUniverse(
                                       setter As Action(Of UniverseData),
                                       getter As Func(Of UniverseData),
                  writeStringToFile As Action(Of String, String),
                  readStringFromFile As Func(Of String, String)) As IUniverseSaveStateModel
        Return New UniverseSaveStateModel(setter, getter, writeStringToFile, readStringFromFile)
    End Function
End Class
