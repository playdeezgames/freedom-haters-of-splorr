Imports FHOS.Persistence

Friend Class UniverseGeneratorModel
    Implements IUniverseGeneratorModel
    Private ReadOnly universe As IUniverse

    Protected Sub New(universe As IUniverse)
        Me.universe = universe
    End Sub
    Friend Shared Function FromUniverse(universe As IUniverse) As IUniverseGeneratorModel
        Return New UniverseGeneratorModel(universe)
    End Function
End Class
