Friend Module GalacticDensities
    Friend Const Dense = "Dense"
    Friend Const Average = "Average"
    Friend Const Sparse = "Sparse"
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, GalacticDensityDescriptor) =
        New Dictionary(Of String, GalacticDensityDescriptor) From
        {
            {Dense, New GalacticDensityDescriptor("Dense", 1, 4, 6)},
            {Average, New GalacticDensityDescriptor("Average", 2, 8, 12)},
            {Sparse, New GalacticDensityDescriptor("Sparse", 3, 12, 18)}
        }
End Module
