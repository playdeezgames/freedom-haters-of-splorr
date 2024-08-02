Public Interface IUniverseGeneratorModel
    Sub Start()
    Sub Generate()
    ReadOnly Property StepsRemaining As Integer
    ReadOnly Property StepsCompleted As Integer
    ReadOnly Property CurrentStep As String
    ReadOnly Property Done As Boolean
End Interface
