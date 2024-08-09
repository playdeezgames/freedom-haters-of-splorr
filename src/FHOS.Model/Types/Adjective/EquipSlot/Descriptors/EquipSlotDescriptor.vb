Friend Class EquipSlotDescriptor
    Sub New(
           equipSlot As String,
           mandatory As Boolean,
           displayName As String,
           category As String)
        Me.EquipSlot = equipSlot
        Me.Mandatory = mandatory
        Me.DisplayName = displayName
        Me.Category = category
    End Sub

    Friend ReadOnly Property EquipSlot As String
    Friend ReadOnly Property DisplayName As String
    Friend ReadOnly Property Category As String
    Friend ReadOnly Property Mandatory As Boolean
End Class
