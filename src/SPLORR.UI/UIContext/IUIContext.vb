Public Interface IUIContext(Of TModel)
    ReadOnly Property Model As TModel
    ReadOnly Property UIPalette As IUIPalette
    ReadOnly Property AvailableWindowSizes As IEnumerable(Of (Width As Integer, Height As Integer))
    ReadOnly Property ViewSize As (Width As Integer, Height As Integer)
    ReadOnly Property ViewCenter As (X As Integer, Y As Integer)
    Function Font(fontName As String) As Font
    'display utilities
    Sub ShowStatusBar(displayBuffer As IPixelSink, font As Font, text As String, foreground As Integer, background As Integer)
    Sub ShowHeader(displayBuffer As IPixelSink, font As Font, text As String, foreground As Integer, background As Integer)
    Sub ShowSplashContent(displayBuffer As IPixelSink, font As Font)
    Sub ShowAboutContent(displayBuffer As IPixelSink, font As Font)
    Function ControlsText(Optional aButton As String = Nothing, Optional bButton As String = Nothing, Optional selectButton As String = Nothing, Optional startButton As String = Nothing) As String
    Sub AbandonGame()
    'save game stuff
    Sub LoadGame(slot As Integer)
    Sub SaveGame(slot As Integer)
    Function DoesSlotExist(slot As Integer) As Boolean
End Interface
