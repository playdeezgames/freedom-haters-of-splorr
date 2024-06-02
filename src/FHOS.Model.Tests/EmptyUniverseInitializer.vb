Friend Class EmptyUniverseInitializer
    Implements IInitializer

    Private ReadOnly stepCount As Integer
    Private _total As Integer
    Private _complete As Integer

    Sub New(stepCount As Integer)
        Me.stepCount = stepCount
    End Sub

    Public ReadOnly Property StepsRemaining As Integer Implements IInitializer.StepsRemaining
        Get
            Return _total - _complete
        End Get
    End Property

    Public ReadOnly Property StepsDone As Integer Implements IInitializer.StepsDone
        Get
            Return _complete
        End Get
    End Property

    Public Sub Start(universe As IUniverse, settings As EmbarkSettings) Implements IInitializer.Start
        _total = stepCount
        _complete = 0
    End Sub

    Public Function Execute() As Boolean Implements IInitializer.Execute
        If _complete >= _total Then
            Return False
        End If
        _complete += 1
        Return True
    End Function
End Class
