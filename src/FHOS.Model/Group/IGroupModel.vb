Public Interface IGroupModel
    ReadOnly Property Name As String
    ReadOnly Property Authority As (LevelName As String, Value As Integer)
    ReadOnly Property Standards As (LevelName As String, Value As Integer)
    ReadOnly Property Conviction As (LevelName As String, Value As Integer)
    ReadOnly Property PlanetCount As Integer
    ReadOnly Property PlanetList As IEnumerable(Of IGroupModel)
    ReadOnly Property SatelliteList As IEnumerable(Of IGroupModel)
    Function RelationNameTo(otherGroup As IGroupModel) As String
    ReadOnly Property Actors As IEnumerable(Of IActorModel)
    ReadOnly Property Pedia As IUniversePediaModel
End Interface
