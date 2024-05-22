Public Interface IUniversePediaModel
    ReadOnly Property FactionList As IEnumerable(Of (Text As String, Faction As IFactionModel))
    ReadOnly Property StarSystems As IEnumerable(Of (Text As String, Place As IPlaceModel))
End Interface
