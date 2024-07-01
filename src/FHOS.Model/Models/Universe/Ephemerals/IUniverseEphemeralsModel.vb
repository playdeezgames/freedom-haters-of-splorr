Public Interface IUniverseEphemeralsModel
    ReadOnly Property SelectedFaction As Stack(Of IGroupModel)
    ReadOnly Property SelectedPlanet As Stack(Of IGroupModel)
    ReadOnly Property SelectedSatellite As Stack(Of IGroupModel)
    ReadOnly Property SelectedStarSystem As Stack(Of IGroupModel)
    Property CurrentOffer As IAvatarTraderOfferModel
    Property CurrentPrice As IAvatarTraderPriceModel
    Property SellQuantity As Integer
End Interface
