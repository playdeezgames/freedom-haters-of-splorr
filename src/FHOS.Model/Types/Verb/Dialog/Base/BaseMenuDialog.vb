Imports FHOS.Data

Friend MustInherit Class BaseMenuDialog
    Inherits BaseDialog
    Private _initialized As Boolean = False
    Private _lines As IEnumerable(Of (Hue As Integer, Text As String)) = Nothing
    Private _menu As IReadOnlyDictionary(Of String, Func(Of IDialog)) = Nothing

    Protected Sub New(actor As Persistence.IActor, finalDialog As IDialog, prompt As String)
        MyBase.New(DialogType.Menu, actor, finalDialog, prompt, Nothing)
    End Sub

    Private Sub Initialize()
        If Not _initialized Then
            _lines = InitializeLines()
            _menu = InitializeMenu()
            _initialized = True
        End If
    End Sub

    Protected MustOverride Function InitializeMenu() As IReadOnlyDictionary(Of String, Func(Of IDialog))
    Protected MustOverride Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))

    Public Overrides ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String))
        Get
            Initialize()
            Return _lines
        End Get
    End Property

    Public Overrides ReadOnly Property Menu As IEnumerable(Of String)
        Get
            Initialize()
            Return _menu.Keys
        End Get
    End Property

    Public Overrides Function Choose(choice As String) As IDialog
        Initialize()
        Dim value As Func(Of IDialog) = Nothing
        If _menu.TryGetValue(choice, value) Then
            Return value()
        End If
        Return Me
    End Function
End Class
