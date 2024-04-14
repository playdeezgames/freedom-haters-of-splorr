Imports System.IO
Imports System.Text.Json
Imports FHOS.Presentation
Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Input
Imports SPLORR.Presentation
Imports SPLORR.UI
Module Program
    Sub Main(args As String())
        Dim context = New FHOSContext(LoadFonts(), (ViewWidth, ViewHeight), New FHOSKeyBindings(KeysFilename))
        Using host As New Host(
            $"{GameTitle}: {GameSubtitle}",
            New FHOSController(
                New FHOSSettings(),
                context),
            (ViewWidth, ViewHeight),
            LoadHues(),
            LoadCommands(context.KeyBindings),
            LoadSfx(),
            LoadMux)
            host.Run()
        End Using
    End Sub
    Private Function LoadCommands(keyBindings As IKeyBindings) As IReadOnlyDictionary(Of String, Func(Of KeyboardState, GamePadState, Boolean))
        Dim keysTable = keyBindings.KeysTable.ToDictionary(Function(x) [Enum].Parse(Of Keys)(x.Key), Function(x) x.Value)
        Dim keysForCommands = keysTable.
            GroupBy(Function(x) x.Value).
            ToDictionary(
                Function(x) x.Key,
                Function(x) x.Select(Function(y) y.Key).ToList())
        Dim result = New Dictionary(Of String, Func(Of KeyboardState, GamePadState, Boolean))
        For Each cmd In gamePadCommandTable.Keys
            result.Add(cmd, MakeCommandHandler(If(keysForCommands.ContainsKey(cmd), keysForCommands(cmd), Array.Empty(Of Keys)().ToList), gamePadCommandTable(cmd)))
        Next
        Return result
    End Function

    Private Function MakeCommandHandler(keys As List(Of Keys), func As Func(Of GamePadState, Boolean)) As Func(Of KeyboardState, GamePadState, Boolean)
        Return Function(k, g)
                   Return func(g) OrElse keys.Any(Function(x) k.IsKeyDown(x))
               End Function
    End Function

    Private Const KeysFilename As String = "Content/keys.json"
    Private Const HueFilename As String = "Content/hue.json"
    Private Const SfxFilename As String = "Content/sfx.json"
    Private Const FontFilename As String = "Content/font.json"
    Private Const MuxFilename As String = "Content/mux.json"
    Private Function LoadHues() As IReadOnlyDictionary(Of Integer, Color)
        Return JsonSerializer.Deserialize(Of Dictionary(Of Integer, Color))(File.ReadAllText(HueFilename))
    End Function
    Private Function LoadSfx() As IReadOnlyDictionary(Of String, String)
        Return JsonSerializer.Deserialize(Of Dictionary(Of String, String))(File.ReadAllText(SfxFilename))
    End Function
    Private Function LoadFonts() As IReadOnlyDictionary(Of String, String)
        Return JsonSerializer.Deserialize(Of Dictionary(Of String, String))(File.ReadAllText(FontFilename))
    End Function
    Private Function LoadMux() As IReadOnlyDictionary(Of String, String)
        Return JsonSerializer.Deserialize(Of Dictionary(Of String, String))(File.ReadAllText(MuxFilename))
    End Function
    Private ReadOnly gamePadCommandTable As IReadOnlyDictionary(Of String, Func(Of GamePadState, Boolean)) =
    New Dictionary(Of String, Func(Of GamePadState, Boolean)) From
    {
        {SPLORR.UI.Command.A, Function(gamePad) gamePad.IsButtonDown(Buttons.A)},
        {SPLORR.UI.Command.B, Function(gamePad) gamePad.IsButtonDown(Buttons.B)},
        {SPLORR.UI.Command.Start, Function(gamePad) gamePad.IsButtonDown(Buttons.Start)},
        {SPLORR.UI.Command.Select, Function(gamePad) gamePad.IsButtonDown(Buttons.Back)},
        {SPLORR.UI.Command.Up, Function(gamePad) gamePad.DPad.Up = ButtonState.Pressed},
        {SPLORR.UI.Command.Down, Function(gamePad) gamePad.DPad.Down = ButtonState.Pressed},
        {SPLORR.UI.Command.Left, Function(gamePad) gamePad.DPad.Left = ButtonState.Pressed},
        {SPLORR.UI.Command.Right, Function(gamePad) gamePad.DPad.Right = ButtonState.Pressed}
    }
End Module
