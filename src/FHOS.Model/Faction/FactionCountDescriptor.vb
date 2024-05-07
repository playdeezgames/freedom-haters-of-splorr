Friend Class FactionCountDescriptor
    Private ReadOnly factionCount As Integer
    Friend ReadOnly Property Name As String

    Public Sub New(factionCount As Integer, name As String)
        Me.factionCount = factionCount
        Me.Name = name
    End Sub
End Class
