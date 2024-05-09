Imports FHOS.Model
Imports FHOS.Persistence
Imports SPLORR.UI

Public Class KnownPlaceListState
    Inherits BasePickerState(Of Model.IUniverseModel, IPlaceModel)
    Friend Shared Property SelectedPlace As IPlaceModel
    Private ReadOnly placeType As String
    Private ReadOnly detailsState As String

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel),
                  headerText As String,
                  cancelGameState As String,
                  placeType As String,
                  detailsState As String)
        MyBase.New(
            parent,
            setState,
            context,
            headerText,
            context.ControlsText(aButton:="Choose", bButton:="Cancel"),
            cancelGameState)
        Me.placeType = placeType
        Me.detailsState = detailsState
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As IPlaceModel))
        SelectedPlace = value.Item
        SetState(detailsState)
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As IPlaceModel))
        Return Context.Model.Avatar.GetKnownPlacesOfType(placeType).ToList
    End Function
End Class
