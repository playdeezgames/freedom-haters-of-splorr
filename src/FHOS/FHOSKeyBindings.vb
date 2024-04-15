Imports System.IO
Imports System.Text.Json
Imports Microsoft.Xna.Framework.Input
Imports SPLORR.UI

Friend Class FHOSKeyBindings
    Implements IKeyBindings

    Private ReadOnly keysFilename As String
    Private _keysTable As Dictionary(Of String, String)


    Public Sub New(keysFilename As String)
        Me.keysFilename = keysFilename
        Reload()
    End Sub

    Public ReadOnly Property KeysTable As IReadOnlyDictionary(Of String, String) Implements IKeyBindings.KeysTable
        Get
            Return _keysTable
        End Get
    End Property

    Public ReadOnly Property UnboundKeys As IEnumerable(Of (Text As String, Item As String)) Implements IKeyBindings.UnboundKeys
        Get
            Dim boundKeys = New HashSet(Of Keys)(KeysTable.Keys.Select(Function(x) [Enum].Parse(Of Keys)(x)))
            Return [Enum].GetValues(Of Keys).Where(Function(x) Not boundKeys.Contains(x)).Select(Function(x) (x.ToString, x.ToString))
        End Get
    End Property

    Public Sub Save() Implements IKeyBindings.Save
        File.WriteAllText(keysFilename, JsonSerializer.Serialize(_keysTable))
    End Sub

    Public Sub RestoreDefaults() Implements IKeyBindings.RestoreDefaults
        _keysTable = New Dictionary(Of String, String) From
            {
                {Keys.Up.ToString, Command.Up},
                {Keys.W.ToString, Command.Up},
                {Keys.Z.ToString, Command.Up},
                {Keys.NumPad8.ToString, Command.Up},
                {Keys.Down.ToString, Command.Down},
                {Keys.S.ToString, Command.Down},
                {Keys.NumPad2.ToString, Command.Down},
                {Keys.Left.ToString, Command.Left},
                {Keys.A.ToString, Command.Left},
                {Keys.Q.ToString, Command.Left},
                {Keys.NumPad4.ToString, Command.Left},
                {Keys.Right.ToString, Command.Right},
                {Keys.D.ToString, Command.Right},
                {Keys.NumPad6.ToString, Command.Right},
                {Keys.Space.ToString, Command.A},
                {Keys.Escape.ToString, Command.B},
                {Keys.NumPad0.ToString, Command.B},
                {Keys.Tab.ToString, Command.Select},
                {Keys.Enter.ToString, Command.Start}
            }
    End Sub

    Public Sub Bind(key As String, command As String) Implements IKeyBindings.Bind
        _keysTable(key) = command
    End Sub

    Public Sub Reload() Implements IKeyBindings.Reload
        Try
            _keysTable = JsonSerializer.Deserialize(Of Dictionary(Of String, String))(File.ReadAllText(keysFilename))
        Catch ex As Exception
            RestoreDefaults()
            Save()
        End Try
    End Sub

    Public Sub Unbind(key As String) Implements IKeyBindings.Unbind
        _keysTable.Remove(key)
    End Sub

    Public Function CanUnbind(key As String) As Boolean Implements IKeyBindings.CanUnbind
        If Not _keysTable.ContainsKey(key) Then
            Return False
        End If
        Dim boundCommand = _keysTable(key)
        Return _keysTable.Values.Where(Function(x) x = boundCommand).Count > 1
    End Function
End Class
