Friend Class EquipSlotDescriptor
    Sub New(
           equipSlot As String,
           displayName As String)
        Me.EquipSlot = equipSlot
        Me.DisplayName = displayName
    End Sub

    Friend ReadOnly Property EquipSlot As String
    Public ReadOnly Property DisplayName As String
End Class
