Imports FHOS.Persistence

Friend Class UniverseEphemeralsModel
    Implements IUniverseEphemeralsModel
    Sub New()
        Me.SelectedFaction = New Stack(Of IGroupModel)
        Me.SelectedPlanet = New Stack(Of IGroupModel)
        Me.SelectedSatellite = New Stack(Of IGroupModel)
        Me.SelectedStarSystem = New Stack(Of IGroupModel)
        Me.CurrentOffer = Nothing
        Me.CurrentPrice = Nothing
    End Sub
    Public ReadOnly Property SelectedFaction As Stack(Of IGroupModel) Implements IUniverseEphemeralsModel.SelectedFaction
    Public ReadOnly Property SelectedPlanet As Stack(Of IGroupModel) Implements IUniverseEphemeralsModel.SelectedPlanet
    Public ReadOnly Property SelectedSatellite As Stack(Of IGroupModel) Implements IUniverseEphemeralsModel.SelectedSatellite
    Public ReadOnly Property SelectedStarSystem As Stack(Of IGroupModel) Implements IUniverseEphemeralsModel.SelectedStarSystem
    Public Property CurrentOffer As IAvatarTraderOfferModel Implements IUniverseEphemeralsModel.CurrentOffer
    Public Property CurrentPrice As IAvatarTraderPriceModel Implements IUniverseEphemeralsModel.CurrentPrice
End Class
