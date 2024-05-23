Public Interface IUniversePediaModel
    ReadOnly Property FactionList As IEnumerable(Of (Text As String, Faction As IFactionModel))
End Interface
