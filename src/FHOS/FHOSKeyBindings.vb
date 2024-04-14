Imports System.IO
Imports System.Text.Json
Imports SPLORR.UI

Friend Class FHOSKeyBindings
    Implements IKeyBindings

    Private keysFilename As String


    Public Sub New(keysFilename As String)
        Me.keysFilename = keysFilename
        KeysTable = JsonSerializer.Deserialize(Of Dictionary(Of String, String))(File.ReadAllText(keysFilename))
    End Sub

    Public ReadOnly Property KeysTable As IReadOnlyDictionary(Of String, String) Implements IKeyBindings.KeysTable
End Class
