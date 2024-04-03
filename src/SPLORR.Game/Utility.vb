Public Module Utility
    Public Function MakeHashSet(Of T)(ParamArray items() As T) As HashSet(Of T)
        Return New HashSet(Of T)(items)
    End Function
    Public Function MakeDictionary(Of T, U)(ParamArray entries() As (First As T, Second As U)) As IReadOnlyDictionary(Of T, U)
        Return entries.ToDictionary(Of T, U)(
            Function(x) x.First, Function(x) x.Second)
    End Function
    Public Function MakeList(Of T)(ParamArray items() As T) As IReadOnlyList(Of T)
        Return items.ToList
    End Function
End Module
