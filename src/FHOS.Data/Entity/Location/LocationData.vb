Public Class LocationData
    Inherits IdentifiedEntityData
    Implements ILocationData
    Public Sub New(
                  connection As SqliteConnection,
                  id As Integer,
                  Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing,
                  Optional metadatas As IReadOnlyDictionary(Of String, String) = Nothing)
        MyBase.New(connection, "Location", id, statistics:=statistics, metadatas:=metadatas)
    End Sub
End Class
