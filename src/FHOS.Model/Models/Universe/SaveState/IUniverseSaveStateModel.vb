Public Interface IUniverseSaveStateModel
    Sub Save(filename As String)
    Sub Load(filename As String)
    Sub Abandon()
End Interface
