Imports System.Runtime.CompilerServices
Imports FHOS.Persistence

Friend Module ItemExtensions
    <Extension>
    Friend Function Descriptor(item As IItem) As ItemTypeDescriptor
        Return ItemTypes.Descriptors(item.EntityType)
    End Function
    <Extension>
    Friend Sub OnEquip(item As IItem, actor As IActor)
        item.Descriptor.Equip(actor, item)
    End Sub
    <Extension>
    Friend Sub OnUnequip(item As IItem, actor As IActor)
        item.Descriptor.Unequip(actor, item)
    End Sub
End Module
