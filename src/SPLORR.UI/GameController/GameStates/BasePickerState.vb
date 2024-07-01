Public MustInherit Class BasePickerState(Of TModel, TItem)
    Inherits BaseGameState(Of TModel)
    Private _menuItems As New List(Of (Text As String, Item As TItem))
    Protected MenuItemIndex As Integer
    Private ReadOnly _statusBarText As String
    Protected Property HeaderText As String
    Protected ReadOnly _cancelGameState As String
    Protected Property _pageSize As Integer?
    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of TModel),
                  headerText As String,
                  statusBarText As String,
                  cancelGameState As String,
                  Optional pageSize As Integer? = Nothing)
        MyBase.New(parent, setState, context)
        _statusBarText = statusBarText
        _cancelGameState = cancelGameState
        Me.HeaderText = headerText
        _pageSize = pageSize
    End Sub
    Public Overrides Sub HandleCommand(cmd As String)
        Select Case cmd
            Case Command.B
                SetState(_cancelGameState)
            Case Command.A, Command.Start
                OnActivateMenuItem(_menuItems(MenuItemIndex))
            Case Command.Up
                MenuItemIndex = (MenuItemIndex + _menuItems.Count - 1) Mod _menuItems.Count
            Case Command.Down, Command.Select
                MenuItemIndex = (MenuItemIndex + 1) Mod _menuItems.Count
            Case Command.Right
                If _pageSize.HasValue Then
                    MenuItemIndex = (MenuItemIndex + _pageSize.Value) Mod _menuItems.Count
                End If
            Case Command.Left
                If _pageSize.HasValue Then
                    MenuItemIndex = (MenuItemIndex + _menuItems.Count - _pageSize.Value) Mod _menuItems.Count
                End If
        End Select
    End Sub
    Protected MustOverride Sub OnActivateMenuItem(value As (Text As String, Item As TItem))
    Protected Overridable Function GetCenterY() As Integer
        Return Context.ViewCenter.Y
    End Function
    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim font = Context.Font(UIFont)
        displayBuffer.Fill((0, GetCenterY() - font.HalfHeight), (Context.ViewSize.Width, font.Height), Context.UIPalette.MenuItem)
        Dim y = GetCenterY() - font.HalfHeight - MenuItemIndex * font.Height
        Dim index = 0
        For Each menuItem In _menuItems
            Dim h = If(index = MenuItemIndex, Context.UIPalette.Background, Context.UIPalette.MenuItem)
            font.WriteCenteredText(displayBuffer, (Context.ViewCenter.X, y), menuItem.Text, h)
            index += 1
            y += font.Height
        Next

        ShowHeader(displayBuffer, font)
        Context.ShowStatusBar(displayBuffer, font, _statusBarText, Context.UIPalette.Background, Context.UIPalette.Footer)
    End Sub

    Private Sub ShowHeader(displayBuffer As IPixelSink, font As Font)
        Dim text = HeaderText
        If _pageSize.HasValue Then
            Dim page = MenuItemIndex \ _pageSize.Value + 1
            Dim pages = (_menuItems.Count + _pageSize.Value - 1) \ _pageSize.Value
            text = $"<= {HeaderText} Pg {page}/{pages} =>"
        End If
        Context.ShowHeader(displayBuffer, font, text, Context.UIPalette.Header, Context.UIPalette.Background)
    End Sub

    Public Overrides Sub OnStart()
        MenuItemIndex = 0
        _menuItems = InitializeMenuItems()
    End Sub
    Protected MustOverride Function InitializeMenuItems() As List(Of (Text As String, Item As TItem))
End Class
