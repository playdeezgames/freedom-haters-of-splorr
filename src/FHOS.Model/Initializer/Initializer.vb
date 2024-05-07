Imports FHOS.Persistence

Friend Module Initializer
    Private _stepCount As Integer
    ReadOnly Property StepsDone As Integer
        Get
            Return _stepCount
        End Get
    End Property
    ReadOnly Property StepsRemaining As Integer
        Get
            Return initializationSteps.Count + finalizationSteps.Count
        End Get
    End Property
    Private ReadOnly initializationSteps As New Queue(Of InitializationStep)
    Private ReadOnly finalizationSteps As New Queue(Of InitializationStep)
    Sub Start(universe As IUniverse, embarkSettings As EmbarkSettings)
        _stepCount = 0
        AddStep(New FactionInitializationStep(universe, embarkSettings), True)
        AddStep(New GalaxyInitializationStep(universe, embarkSettings, New NameGenerator), True)
        AddStep(New FactionizeGalaxyStep(universe), True)
    End Sub
    Private Sub AddStep(initializationStep As InitializationStep, Optional isFinalizer As Boolean = False)
        If isFinalizer Then
            finalizationSteps.Enqueue(initializationStep)
        Else
            initializationSteps.Enqueue(initializationStep)
        End If
    End Sub
    Public Function Execute() As Boolean
        If initializationSteps.Any Then
            _stepCount += 1
            Dim initializationStep = initializationSteps.Dequeue()
            initializationStep.DoStep(AddressOf AddStep)
            Return True
        End If
        If finalizationSteps.Any Then
            _stepCount += 1
            Dim initializationStep = finalizationSteps.Dequeue()
            initializationStep.DoStep(AddressOf AddStep)
            Return True
        End If
        Return False
    End Function
End Module
