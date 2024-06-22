Public Class LocationData
    Inherits IdentifiedEntityData
    Implements ILocationData
    Public Sub New(
                  connection As SqliteConnection,
                  id As Integer,
                  Optional metadatas As IReadOnlyDictionary(Of String, String) = Nothing)
        MyBase.New(connection, "Location", id, metadatas:=metadatas)
    End Sub
End Class
