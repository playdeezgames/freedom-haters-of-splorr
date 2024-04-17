Public Interface IGalacticAgeModel
    Sub SetAge(age As String)
    ReadOnly Property CurrentName As String
    ReadOnly Property Options As IEnumerable(Of (Text As String, Item As String))
    ReadOnly Property Value As String
End Interface
