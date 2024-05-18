Imports FHOS.Persistence
Imports System.Runtime.CompilerServices

Module StoreExtensions
    <Extension>
    Function NeedsTopOff(store As IStore) As Boolean
        If Not store.MaximumValue.HasValue Then
            Return False
        End If
        Return store.CurrentValue < store.MaximumValue.Value
    End Function
    <Extension>
    Function TopOffAmount(store As IStore) As Integer?
        If Not store.MaximumValue.HasValue Then
            Return Nothing
        End If
        Return store.MinimumValue.Value - store.CurrentValue
    End Function
End Module
