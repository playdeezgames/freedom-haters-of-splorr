Imports FHOS.Persistence

Friend Class Initializer
    Implements IInitializer
    Private _stepCount As Integer
    ReadOnly Property StepsDone As Integer Implements IInitializer.StepsDone
        Get
            Return _stepCount
        End Get
    End Property
    ReadOnly Property StepsRemaining As Integer Implements IInitializer.StepsRemaining
        Get
            Return initializationSteps.Count + finalizationSteps.Count
        End Get
    End Property

    Public ReadOnly Property CurrentStep As String Implements IInitializer.CurrentStep
        Get
            If initializationSteps.Any Then
                Return initializationSteps.Peek.Name
            End If
            If finalizationSteps.Any Then
                Return finalizationSteps.Peek.Name
            End If
            Return "Done!"
        End Get
    End Property

    Private ReadOnly initializationSteps As New Queue(Of InitializationStep)
    Private ReadOnly finalizationSteps As New Queue(Of InitializationStep)
    Sub Start(universe As IUniverse, embarkSettings As EmbarkSettings) Implements IInitializer.Start
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
    Public Function Execute() As Boolean Implements IInitializer.Execute
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
End Class
