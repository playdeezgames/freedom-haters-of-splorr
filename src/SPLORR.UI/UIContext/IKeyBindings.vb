Public Interface IKeyBindings
    ReadOnly Property KeysTable As IReadOnlyDictionary(Of String, String)
    ReadOnly Property UnboundKeys As IEnumerable(Of (Text As String, Item As String))
    Sub Save()
    Sub RestoreDefaults()
    Sub Bind(key As String, command As String)
    Sub Reload()
    Sub Unbind(key As String)
    Function CanUnbind(key As String) As Boolean
End Interface
