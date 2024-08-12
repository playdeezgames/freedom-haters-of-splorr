Friend Module GroupValues
    Friend ReadOnly Descriptors As IReadOnlyList(Of GroupValueDescriptor) =
        New List(Of GroupValueDescriptor) From
        {
            New UnityInDiversityGroupValueDescriptor(),
            New RelentlessInnovationGroupValueDescriptor(),
            New SovereignFreedomGroupValueDescriptor(),
            New SustainableHarmonyGroupValueDescriptor(),
            New AbsoluteOrderGroupValueDescriptor(),
            New CollectiveProsperityGroupValueDescriptor(),
            New ExploratorySpiritGroupValueDescriptor(),
            New CulturalPreservationGroupValueDescriptor(),
            New TechnocraticEfficiencyGroupValueDescriptor(),
            New MartialHonorGroupValueDescriptor()
        }
End Module
