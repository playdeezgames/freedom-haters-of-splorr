Imports FHOS.Persistence

Public Interface IInitializer
    ReadOnly Property StepsRemaining As Integer
    ReadOnly Property StepsDone As Integer
    Sub Start(universe As IUniverse, settings As EmbarkSettings)
    Function Execute() As Boolean
End Interface
