Imports FHOS.Persistence

Friend MustInherit Class InitializationStep
    MustOverride ReadOnly Property Name As String
    MustOverride Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
End Class
