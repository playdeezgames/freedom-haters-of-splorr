Public Class LocationData
    Inherits IdentifiedEntityData
    Implements ILocationData
    Public Sub New(id As Integer, Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing)
        MyBase.New(id, statistics:=statistics)
    End Sub
End Class
