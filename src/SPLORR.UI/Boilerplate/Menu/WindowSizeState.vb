Friend Class WindowSizeState(Of TModel)
    Inherits BasePickerState(Of TModel, (Width As Integer, Height As Integer))
    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of TModel))
        MyBase.New(parent, setState, context, "<placeholder>", context.ControlsText("Choose", "Cancel"), BoilerplateState.Options)
    End Sub
    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As (Width As Integer, Height As Integer)))
        Dim width = value.Item.Width
        Dim height = value.Item.Height
        Parent.Size = (width, height)
        SaveConfig()
        HeaderText = $"Current Size: {Size.Width}x{Size.Height}"
    End Sub
    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As (Width As Integer, Height As Integer)))
        Dim currentSize = $"{Size.Width}x{Size.Height}"
        HeaderText = $"Current Size: {currentSize}"
        Dim menuItems As List(Of (Text As String, Item As (Width As Integer, Height As Integer))) = Context.AvailableWindowSizes.
            Select(Function(x) ($"{x.Width}x{x.Height}", (x.Width, x.Height))).
            ToList
        MenuItemIndex = Math.Max(0, menuItems.FindIndex(Function(x) x.Text = currentSize))
        Return menuItems
    End Function
End Class
