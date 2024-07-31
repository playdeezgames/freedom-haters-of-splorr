Public Interface IAvatarModel
    ReadOnly Property Bio As IAvatarBioModel
    ReadOnly Property State As IAvatarStateModel
    ReadOnly Property Status As IAvatarStatusModel
    ReadOnly Property Verbs As IAvatarVerbsModel
    ReadOnly Property Vessel As IAvatarVesselModel
    ReadOnly Property Jools As Integer
    ReadOnly Property Inventory As IAvatarInventoryModel
    ReadOnly Property Equipment As IAvatarEquipmentModel
    'TODO: these become part of "yokes"
    ReadOnly Property Interaction As IAvatarInteractionModel
    ReadOnly Property StarGate As IAvatarStarGateModel
    ReadOnly Property Trader As IAvatarTraderModel
    ReadOnly Property Shipyard As IAvatarShipyardModel
End Interface
