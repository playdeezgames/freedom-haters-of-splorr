Imports System.Text.Json.Serialization

Public MustInherit Class EntityData
    Property Flags As New HashSet(Of String)
    Property Statistics As New Dictionary(Of String, Integer)
    Property Metadatas As New Dictionary(Of String, String)
End Class
