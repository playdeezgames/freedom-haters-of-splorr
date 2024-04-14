Imports System.IO
Imports System.Text.Json
Public MustInherit Class UIContext(Of TModel)
    Implements IUIContext(Of TModel)
    Private ReadOnly fonts As New Dictionary(Of String, Font)
    ReadOnly Property ViewSize As (Width As Integer, Height As Integer) Implements IUIContext(Of TModel).ViewSize
    Public MustOverride ReadOnly Property AvailableWindowSizes As IEnumerable(Of (Integer, Integer)) Implements IUIContext(Of TModel).AvailableWindowSizes
    Public ReadOnly Property Model As TModel Implements IUIContext(Of TModel).Model
    Public ReadOnly Property ViewCenter As (X As Integer, Y As Integer) Implements IUIContext(Of TModel).ViewCenter
        Get
            Return (ViewSize.Width \ 2, ViewSize.Height \ 2)
        End Get
    End Property

    Public ReadOnly Property UIPalette As IUIPalette Implements IUIContext(Of TModel).UIPalette

    Public ReadOnly Property KeyBindings As IKeyBindings Implements IUIContext(Of TModel).KeyBindings

    Sub New(
           game As TModel,
           fontFilenames As IReadOnlyDictionary(Of String, String),
           viewSize As (Width As Integer, Height As Integer),
           palette As IUIPalette,
           keyBindings As IKeyBindings)
        Me.Model = game
        Me.ViewSize = viewSize
        For Each entry In fontFilenames
            fonts(entry.Key) = New Font(JsonSerializer.Deserialize(Of FontData)(File.ReadAllText(entry.Value)))
        Next
        Me.UIPalette = palette
        Me.KeyBindings = keyBindings
    End Sub
    Public Sub ShowStatusBar(displayBuffer As IPixelSink, font As Font, text As String, foreground As Integer, background As Integer) Implements IUIContext(Of TModel).ShowStatusBar
        displayBuffer.Fill((0, ViewSize.Height - font.Height), (ViewSize.Width, font.Height), background)
        font.WriteCenteredText(displayBuffer, (ViewCenter.X, ViewSize.Height - font.Height), text, foreground)
    End Sub
    Public Function Font(gameFont As String) As Font Implements IUIContext(Of TModel).Font
        Return fonts(gameFont)
    End Function
    Public MustOverride Sub ShowSplashContent(displayBuffer As IPixelSink, font As Font) Implements IUIContext(Of TModel).ShowSplashContent
    Public Sub ShowHeader(displayBuffer As IPixelSink, font As Font, text As String, foreground As Integer, background As Integer) Implements IUIContext(Of TModel).ShowHeader
        displayBuffer.Fill((0, 0), (ViewSize.Width, font.Height), background)
        font.WriteCenteredText(displayBuffer, (ViewCenter.X, 0), text, foreground)
    End Sub
    Public Function ControlsText(
                                Optional aButton As String = Nothing,
                                Optional bButton As String = Nothing,
                                Optional selectButton As String = Nothing,
                                Optional startButton As String = Nothing) As String Implements IUIContext(Of TModel).ControlsText
        Dim items As New List(Of String)
        If aButton IsNot Nothing Then
            items.Add($"[A] {aButton}")
        End If
        If bButton IsNot Nothing Then
            items.Add($"[B] {bButton}")
        End If
        If selectButton IsNot Nothing Then
            items.Add($"[Select] {selectButton}")
        End If
        If startButton IsNot Nothing Then
            items.Add($"[Start] {startButton}")
        End If
        Return String.Join(" | ", items.ToArray)
    End Function
    Public MustOverride Sub ShowAboutContent(displayBuffer As IPixelSink, font As Font) Implements IUIContext(Of TModel).ShowAboutContent
    Public MustOverride Sub AbandonGame() Implements IUIContext(Of TModel).AbandonGame
    Public MustOverride Sub LoadGame(slot As Integer) Implements IUIContext(Of TModel).LoadGame
    Public MustOverride Sub SaveGame(slot As Integer) Implements IUIContext(Of TModel).SaveGame
    Public MustOverride Function DoesSlotExist(slot As Integer) As Boolean Implements IUIContext(Of TModel).DoesSlotExist
End Class
