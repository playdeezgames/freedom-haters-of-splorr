Public Interface IUniverseModel
    ReadOnly Property SelectedFaction As Stack(Of IGroupModel)
    ReadOnly Property SelectedPlanet As Stack(Of IGroupModel)
    ReadOnly Property SelectedSatellite As Stack(Of IGroupModel)
    ReadOnly Property SelectedStarSystem As Stack(Of IGroupModel)
    Property CurrentOffer As IAvatarTraderOfferModel
    Property CurrentPrice As IAvatarTraderPriceModel

    ReadOnly Property Generator As IUniverseGeneratorModel
    ReadOnly Property Settings As IUniverseSettingsModel
    ReadOnly Property State As IUniverseStateModel
    ReadOnly Property Pedia As IUniversePediaModel
    ReadOnly Property Ephemerals As IUniverseEphemeralsModel
    Sub Save(filename As String) 'pull
    Sub Load(filename As String) 'pull
    Sub Abandon() 'pull
End Interface
