Public Class LocationData
    Inherits EntityData
    Implements ILocationData
    Public Sub New(Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing)
        MyBase.New(statistics:=statistics)
    End Sub
End Class
