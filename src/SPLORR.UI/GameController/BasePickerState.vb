Public MustInherit Class BasePickerState(Of TModel, TItem)
    Inherits BaseGameState(Of TModel)
    Private _menuItems As New List(Of (Text As String, Item As TItem))
    Protected MenuItemIndex As Integer
    Private ReadOnly _statusBarText As String
    Protected Property HeaderText As String
    Private ReadOnly _cancelGameState As String
    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of TModel),
                  headerText As String,
                  statusBarText As String,
                  cancelGameState As String)
        MyBase.New(parent, setState, context)
        _statusBarText = statusBarText
        _cancelGameState = cancelGameState
        Me.HeaderText = headerText
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
        End Select
    End Sub
    Protected MustOverride Sub OnActivateMenuItem(value As (Text As String, Item As TItem))
    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim font = Context.Font(UIFont)
        displayBuffer.Fill((0, Context.ViewCenter.Y - font.HalfHeight), (Context.ViewSize.Width, font.Height), Context.UIPalette.MenuItem)
        Dim y = Context.ViewCenter.Y - font.HalfHeight - MenuItemIndex * font.Height
        Dim index = 0
        For Each menuItem In _menuItems
            Dim x = Context.ViewCenter.X - font.TextWidth(menuItem.Text) \ 2
            Dim h = If(index = MenuItemIndex, Context.UIPalette.Background, Context.UIPalette.MenuItem)
            font.WriteText(displayBuffer, (x, y), menuItem.Text, h)
            index += 1
            y += font.Height
        Next
        Context.ShowHeader(displayBuffer, font, HeaderText, Context.UIPalette.Header, Context.UIPalette.Background)
        Context.ShowStatusBar(displayBuffer, font, _statusBarText, Context.UIPalette.Background, Context.UIPalette.Footer)
    End Sub
    Public Overrides Sub OnStart()
        MenuItemIndex = 0
        _menuItems = InitializeMenuItems()
    End Sub
    Protected MustOverride Function InitializeMenuItems() As List(Of (Text As String, Item As TItem))
End Class
