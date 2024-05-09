Imports FHOS.Persistence

Friend MustInherit Class InitializationStep
    MustOverride Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
End Class
