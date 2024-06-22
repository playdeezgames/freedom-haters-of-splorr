Public Class LocationData
    Inherits IdentifiedEntityData
    Implements ILocationData
    Public Sub New(
                  connection As SqliteConnection,
                  id As Integer)
        MyBase.New(connection, "Location", id)
    End Sub
End Class
