Public Interface IUniversePediaModel
    ReadOnly Property FactionList As IEnumerable(Of (Text As String, Faction As IGroupModel))
End Interface
