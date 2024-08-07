﻿Imports FHOS.Data

Friend Class GroupDataClient
    Inherits NamedEntityDataClient(Of GroupData)
    Public Sub New(
                  universeData As Data.UniverseData,
                  factionId As Integer)
        MyBase.New(
            universeData,
            factionId,
            Function(u, i) u.Groups(i),
            Sub(u, i) u.Groups.Remove(i))
    End Sub
End Class
