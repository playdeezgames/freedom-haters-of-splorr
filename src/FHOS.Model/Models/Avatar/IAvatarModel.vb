Public Interface IAvatarModel
    ReadOnly Property Bio As IAvatarBioModel
    ReadOnly Property State As IAvatarStateModel
    ReadOnly Property Status As IAvatarStatusModel
    ReadOnly Property Verbs As IAvatarVerbsModel
    ReadOnly Property Vessel As IAvatarVesselModel
    ReadOnly Property Jools As Integer
    ReadOnly Property Inventory As IAvatarInventoryModel
    ReadOnly Property Equipment As IAvatarEquipmentModel
    ReadOnly Property Yokes As IAvatarYokesModel
End Interface
