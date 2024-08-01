Imports FHOS.Persistence

Public Class UniverseSaveStateModel
    Implements IUniverseSaveStateModel

    Private ReadOnly universe As IUniverse

    Public Sub New(universe As IUniverse)
        Me.universe = universe
    End Sub

    Friend Shared Function FromUniverse(universe As IUniverse) As IUniverseSaveStateModel
        Return New UniverseSaveStateModel(universe)
    End Function
End Class
