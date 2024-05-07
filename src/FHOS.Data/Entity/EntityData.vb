Imports System.Text.Json.Serialization

Public MustInherit Class EntityData
    <JsonPropertyName("f1")>
    Property Flags As New HashSet(Of String)
    <JsonPropertyName("s1")>
    Property Statistics As New Dictionary(Of String, Integer)
    <JsonPropertyName("m1")>
    Property Metadatas As New Dictionary(Of String, String)
End Class
