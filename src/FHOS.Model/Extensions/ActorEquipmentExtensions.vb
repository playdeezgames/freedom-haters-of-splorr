Imports System.Runtime.CompilerServices
Imports FHOS.Persistence

Friend Module ActorEquipmentExtensions
    <Extension>
    Friend Function GetRequiredSlots(actorEquipment As IActorEquipment) As IEnumerable(Of String)
        Return actorEquipment.Actor.Descriptor.EquipSlots.Where(Function(x) x.Value).Select(Function(x) x.Key)
    End Function
    <Extension>
    Friend Function GetInstallableSlots(actorEquipment As IActorEquipment) As IEnumerable(Of String)
        Return actorEquipment.Actor.Descriptor.EquipSlots.Where(Function(x) actorEquipment.GetSlot(x.Key) Is Nothing).Select(Function(x) x.Key)
    End Function
    <Extension>
    Friend Function GetUninstallableSlots(actorEquipment As IActorEquipment) As IEnumerable(Of String)
        Return actorEquipment.Actor.Descriptor.EquipSlots.Where(Function(x) actorEquipment.GetSlot(x.Key) IsNot Nothing AndAlso Not x.Value).Select(Function(x) x.Key)
    End Function
End Module
