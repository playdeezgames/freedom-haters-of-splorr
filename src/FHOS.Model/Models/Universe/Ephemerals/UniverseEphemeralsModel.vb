Imports FHOS.Persistence

Friend Class UniverseEphemeralsModel
    Implements IUniverseEphemeralsModel

    Private universe As IUniverse

    Public Sub New(universe As IUniverse)
        Me.universe = universe
    End Sub

    Friend Shared Function FromUniverse(universe As IUniverse) As IUniverseEphemeralsModel
        Return New UniverseEphemeralsModel(universe)
    End Function
End Class
