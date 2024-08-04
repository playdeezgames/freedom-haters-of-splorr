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
    <Extension>
    Friend Sub SetDestinationPlanet(item As IItem, destination As IGroup)
        item.Statistics(StatisticTypes.DestinationPlanetId) = destination.Id
    End Sub
    <Extension>
    Friend Function GetDestinationPlanet(item As IItem) As IGroup
        Dim groupId = item.Statistics(StatisticTypes.DestinationPlanetId)
        If Not groupId.HasValue Then
            Return Nothing
        End If
        Return item.Universe.Groups.Single(Function(x) x.Id = groupId.Value)
    End Function
    <Extension>
    Friend Sub SetJoolsReward(item As IItem, reward As Integer)
        item.Statistics(StatisticTypes.JoolsReward) = reward
    End Sub
    <Extension>
    Friend Function GetJoolsReward(item As IItem) As Integer
        Return If(item.Statistics(StatisticTypes.JoolsReward), 0)
    End Function
    <Extension>
    Friend Sub SetReputationBonus(item As IItem, bonus As Integer)
        item.Statistics(StatisticTypes.ReputationBonus) = bonus
    End Sub
    <Extension>
    Friend Sub SetReputationPenalty(item As IItem, penalty As Integer)
        item.Statistics(StatisticTypes.ReputationPenalty) = penalty
    End Sub
    <Extension>
    Friend Function EntityName(item As IItem) As String
        Return item.Descriptor.GetEntityName(item)
    End Function
    <Extension>
    Friend Function ItemTypeName(item As IItem) As String
        Return item.Descriptor.Name
    End Function
    <Extension>
    Friend Function GetRecipient(item As IItem) As String
        Return item.Metadatas(MetadataTypes.Recipient)
    End Function
End Module
