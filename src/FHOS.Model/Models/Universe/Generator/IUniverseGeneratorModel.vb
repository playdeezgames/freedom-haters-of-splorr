Public Interface IUniverseGeneratorModel
    Sub Start()
    Sub Generate()
    ReadOnly Property StepsRemaining As Integer 'gang to steps completed
    ReadOnly Property StepsCompleted As Integer 'gang to steps remaining
    ReadOnly Property Done As Boolean
End Interface
