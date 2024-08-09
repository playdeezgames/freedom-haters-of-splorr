Imports System.Runtime.CompilerServices
Imports FHOS.Persistence

Friend Module ActorEquipmentExtensions
    <Extension>
    Friend Function GetRequiredSlots(actorEquipment As IActorEquipment) As IEnumerable(Of String)
        Return actorEquipment.
            Actor.
            Descriptor.
            EquipSlots.
            Where(Function(x) EquipSlots.Descriptors(x).Mandatory)
    End Function
    <Extension>
    Friend Function GetInstallableSlots(actorEquipment As IActorEquipment) As IEnumerable(Of String)
        Return actorEquipment.
            Actor.
            Descriptor.
            EquipSlots.
            Where(Function(x) actorEquipment.GetSlot(x) Is Nothing)
    End Function
    <Extension>
    Friend Function GetUninstallableSlots(actorEquipment As IActorEquipment) As IEnumerable(Of String)
        Return actorEquipment.
            Actor.
            Descriptor.
            EquipSlots.
            Where(Function(x) actorEquipment.GetSlot(x) IsNot Nothing AndAlso Not EquipSlots.Descriptors(x).Mandatory)
    End Function
End Module
