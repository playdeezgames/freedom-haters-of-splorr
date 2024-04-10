Public Class WorldData
    Property AvatarId As Integer?
    Property Maps As New List(Of MapData)
    Property RecycledMaps As New HashSet(Of Integer)
    Property Characters As New List(Of CharacterData)
    Property RecycledCharacters As New HashSet(Of Integer)
    Property Cells As New List(Of CellData)
    Property RecycledCells As New HashSet(Of Integer)
End Class
