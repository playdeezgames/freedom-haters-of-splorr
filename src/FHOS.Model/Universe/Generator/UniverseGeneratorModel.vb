Imports FHOS.Data
Imports FHOS.Persistence

Friend Class UniverseGeneratorModel
    Implements IUniverseGeneratorModel

    Private ReadOnly dataResetter As Action
    Private ReadOnly universeSource As Func(Of IUniverse)
    Private ReadOnly _settings As EmbarkSettings


    Protected Sub New(dataResetter As Action, universeSource As Func(Of IUniverse), settings As EmbarkSettings)
        Me.dataResetter = dataResetter
        Me.universeSource = universeSource
        Me._settings = settings
    End Sub

    Public Sub Start() Implements IUniverseGeneratorModel.Start
        dataResetter()
        universeSource().Turn = 1
        Initializer.Start(universeSource(), _settings)
    End Sub

    Public Sub Generate() Implements IUniverseGeneratorModel.Generate
        Dim endTime = DateTimeOffset.Now.AddSeconds(0.01)
        Do
            Initializer.Execute()
        Loop Until DateTimeOffset.Now >= endTime
    End Sub

    Friend Shared Function MakeGenerator(
                                        dataResetter As Action,
                                        universeSource As Func(Of IUniverse),
                                        settings As EmbarkSettings) As IUniverseGeneratorModel
        Return New UniverseGeneratorModel(dataResetter, universeSource, settings)
    End Function

    Public ReadOnly Property StepsRemaining As Integer Implements IUniverseGeneratorModel.StepsRemaining
        Get
            Return Initializer.StepsRemaining
        End Get
    End Property

    Public ReadOnly Property StepsCompleted As Integer Implements IUniverseGeneratorModel.StepsCompleted
        Get
            Return Initializer.StepsDone
        End Get
    End Property

    Public ReadOnly Property Done As Boolean Implements IUniverseGeneratorModel.Done
        Get
            Return Initializer.StepsRemaining = 0
        End Get
    End Property
End Class
