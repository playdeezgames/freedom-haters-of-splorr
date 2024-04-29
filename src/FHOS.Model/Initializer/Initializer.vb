Imports FHOS.Persistence

Friend Module Initializer
    Private ReadOnly initializationSteps As New Queue(Of InitializationStep)
    Sub Start(universe As IUniverse, embarkSettings As EmbarkSettings)
        AddStep(New UniverseInitializationStep(universe, embarkSettings))
    End Sub
    Private Sub AddStep(initializationStep As InitializationStep)
        initializationSteps.Enqueue(initializationStep)
    End Sub
    Public Function Execute() As Boolean
        If initializationSteps.Any Then
            Dim initializationStep = initializationSteps.Dequeue()
            initializationStep.DoStep(AddressOf AddStep)
            Return True
        End If
        Return False
    End Function
    Public Sub Run(universe As IUniverse, embarkSettings As EmbarkSettings)
        Dim timeStart = DateTimeOffset.Now
        Start(universe, embarkSettings)
        Do
        Loop While Execute()
        Dim timeElapsed = DateTimeOffset.Now - timeStart
        Debug.Print($"Universe Created in {timeElapsed}!")
    End Sub
End Module
