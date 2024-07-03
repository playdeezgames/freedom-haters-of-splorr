Public MustInherit Class IdentifiedEntityData
    Inherits EntityData
    Public Sub New(
                  id As Integer,
                  Optional flags As ISet(Of String) = Nothing,
                  Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing,
                  Optional metadatas As IReadOnlyDictionary(Of String, String) = Nothing)
        MyBase.New(statistics:=statistics, flags:=flags, metadatas:=metadatas)
        Me.Id = id
    End Sub

    Public ReadOnly Property Id As Integer
End Class
