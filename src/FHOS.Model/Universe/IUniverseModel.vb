Public Interface IUniverseModel
    Sub Save(filename As String)
    Sub Load(filename As String)
    Sub Abandon()
    ReadOnly Property Generator As IUniverseGeneratorModel
    ReadOnly Property Settings As IUniverseSettingsModel
    ReadOnly Property State As IUniverseStateModel
    ReadOnly Property Pedia As IUniversePediaModel
End Interface
