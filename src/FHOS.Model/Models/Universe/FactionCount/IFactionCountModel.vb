Public Interface IFactionCountModel
    Sub SetFactionCount(factionCount As Integer)
    ReadOnly Property CurrentName As String
    ReadOnly Property Current As Integer
    ReadOnly Property Options As IEnumerable(Of (Text As String, Item As Integer))
End Interface
