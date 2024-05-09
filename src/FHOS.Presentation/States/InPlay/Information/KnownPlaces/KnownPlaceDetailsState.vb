Imports FHOS.Model
Imports SPLORR.UI

Public MustInherit Class KnownPlaceDetailsState
    Inherits BaseGameState(Of Model.IUniverseModel)
    Protected ReadOnly Property Place As IPlaceModel
        Get
            Return KnownPlaceListState.SelectedPlace
        End Get
    End Property
    Protected MustOverride ReadOnly Property HeaderText As String
    Private ReadOnly cancelGameState As String
    Protected MustOverride ReadOnly Property Details As IEnumerable(Of (Text As String, Hue As Integer))
    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel),
                  cancelGameState As String)
        MyBase.New(
            parent,
            setState,
            context)
        Me.cancelGameState = cancelGameState
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        SetState(cancelGameState)
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim starSystem As IPlaceModel = KnownPlaceListState.SelectedPlace
        With Context
            Dim font = .Font(UIFontName)
            .ShowHeader(
                displayBuffer,
                font,
                HeaderText,
                .UIPalette.Header,
                .UIPalette.Background)
            Dim position = (.ViewCenter.X, font.Height)
            For Each detail In Details
                position = font.WriteCenteredTextLines(displayBuffer, position, .ViewSize.Width, detail.Text, detail.Hue)
            Next
            .ShowStatusBar(
                            displayBuffer,
                            font,
                            .ControlsText(bButton:="Cancel"),
                            .UIPalette.Background,
                            .UIPalette.Footer)
        End With
    End Sub
End Class
