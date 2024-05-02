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
            Return initializationSteps.Count
        End Get
    End Property
    Private ReadOnly initializationSteps As New Queue(Of InitializationStep)
    Sub Start(universe As IUniverse, embarkSettings As EmbarkSettings)
        _stepCount = 0
        AddStep(New GalaxyInitializationStep(universe, embarkSettings, New NameGenerator))
    End Sub
    Private Sub AddStep(initializationStep As InitializationStep)
        initializationSteps.Enqueue(initializationStep)
    End Sub
    Public Function Execute() As Boolean
        If initializationSteps.Any Then
            _stepCount += 1
            Dim initializationStep = initializationSteps.Dequeue()
            initializationStep.DoStep(AddressOf AddStep)
            Return True
        End If
        Return False
    End Function
End Module
