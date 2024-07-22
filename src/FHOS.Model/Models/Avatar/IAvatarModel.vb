Public Interface IAvatarModel
    ReadOnly Property Bio As IAvatarBioModel
    ReadOnly Property Interaction As IAvatarInteractionModel
    ReadOnly Property Stack As IAvatarStackModel
    ReadOnly Property StarGate As IAvatarStarGateModel
    ReadOnly Property Trader As IAvatarTraderModel
    ReadOnly Property Shipyard As IAvatarShipyardModel
    ReadOnly Property State As IAvatarStateModel
    ReadOnly Property Status As IAvatarStatusModel
    ReadOnly Property Verbs As IAvatarVerbsModel
    ReadOnly Property Vessel As IAvatarVesselModel
    ReadOnly Property Jools As Integer
    ReadOnly Property Inventory As IAvatarInventoryModel
    ReadOnly Property Equipment As IAvatarEquipmentModel
End Interface
