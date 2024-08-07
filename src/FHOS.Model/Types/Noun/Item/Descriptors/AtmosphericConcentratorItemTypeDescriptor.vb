﻿Imports FHOS.Data
Imports FHOS.Persistence

Friend Class AtmosphericConcentratorItemTypeDescriptor
    Inherits ItemTypeDescriptor

    Public Sub New()
        MyBase.New(
            ItemTypes.AtmosphericConcentrator,
            "Atmospheric Concentrator",
            price:=5000,
            installFee:=25,
            uninstallFee:=15)
    End Sub
    Friend Overrides Function CanEquip(equipSlot As String) As Boolean
        Return EquipSlots.Descriptors(equipSlot).Category = EquipSlotCategories.AccessoryCategory
    End Function

    Public Overrides ReadOnly Property Description(item As IItem) As String
        Get
            Return "This device allows a vessel to replenish their oxygen from a planet's atmosphere."
        End Get
    End Property

    Protected Overrides Sub Initialize(item As IItem)
        item.Flags(FlagTypes.CanRefillOxygen) = True
    End Sub

    Public Overrides Function Dialogs(actor As IActor, item As IItem, finalDialog As IDialog) As IReadOnlyDictionary(Of String, IDialog)
        Return New Dictionary(Of String, IDialog)
    End Function
End Class
