Public Interface IUniverseModel
    ReadOnly Property Generator As IUniverseGeneratorModel
    ReadOnly Property Settings As IUniverseSettingsModel
    ReadOnly Property State As IUniverseStateModel
    ReadOnly Property Pedia As IUniversePediaModel
    ReadOnly Property SaveState As IUniverseSaveStateModel
    Sub Save(filename As String) 'pull
    Sub Load(filename As String) 'pull
    Sub Abandon() 'pull
End Interface
