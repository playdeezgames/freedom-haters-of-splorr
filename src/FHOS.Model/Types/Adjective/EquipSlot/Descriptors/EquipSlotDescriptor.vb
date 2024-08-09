Friend Class EquipSlotDescriptor
    Sub New(
           equipSlot As String,
           displayName As String,
           category As String)
        Me.EquipSlot = equipSlot
        Me.DisplayName = displayName
        Me.Category = category
    End Sub

    Friend ReadOnly Property EquipSlot As String
    Friend ReadOnly Property DisplayName As String
    Friend ReadOnly Property Category As String
End Class
