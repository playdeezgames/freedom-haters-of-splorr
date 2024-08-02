Public Class OneStepUniverseInitializer
    Implements IInitializer

    Private ReadOnly doStuff As Action(Of IUniverse, EmbarkSettings)
    Sub New(doStuff As Action(Of IUniverse, EmbarkSettings))
        Me.doStuff = doStuff
    End Sub

    Public ReadOnly Property StepsRemaining As Integer Implements IInitializer.StepsRemaining
        Get
            Return 0
        End Get
    End Property

    Public ReadOnly Property StepsDone As Integer Implements IInitializer.StepsDone
        Get
            Return 0
        End Get
    End Property

    Public ReadOnly Property CurrentStep As String Implements IInitializer.CurrentStep
        Get
            Return "One Step Universe Initializer"
        End Get
    End Property

    Public Sub Start(universe As IUniverse, settings As EmbarkSettings) Implements IInitializer.Start
        doStuff(universe, settings)
    End Sub

    Public Function Execute() As Boolean Implements IInitializer.Execute
        Return False
    End Function
End Class
