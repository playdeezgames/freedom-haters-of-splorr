Public Interface IWorldModel
    Sub Save(filename As String)
    Sub Load(filename As String)
    Sub Abandon()
    Sub Embark()
    ReadOnly Property Board As IReadOnlyDictionary(Of Integer, IReadOnlyDictionary(Of Integer, (TerrainType As String, Character As (CharacterType As String, Facing As Integer))))
End Interface
