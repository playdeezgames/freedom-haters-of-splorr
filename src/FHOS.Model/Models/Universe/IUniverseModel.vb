Public Interface IUniverseModel
    ReadOnly Property Generator As IUniverseGeneratorModel
    ReadOnly Property Settings As IUniverseSettingsModel
    ReadOnly Property State As IUniverseStateModel
    ReadOnly Property Pedia As IUniversePediaModel
    ReadOnly Property SaveState As IUniverseSaveStateModel
End Interface
