﻿Imports FHOS.Persistence

Friend Class AvatarInitializationStep
    Inherits InitializationStep
    ReadOnly universe As IUniverse
    ReadOnly embarkSettings As EmbarkSettings
    Sub New(universe As IUniverse, embarkSettings As EmbarkSettings)
        Me.universe = universe
        Me.embarkSettings = embarkSettings
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Initializing Avatar..."
        End Get
    End Property

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim actor = universe.Avatar.Actor
        actor.Yokes.Store(YokeTypes.Wallet) = universe.Factory.CreateStore(
            StartingWealthLevels.Descriptors(embarkSettings.StartingWealthLevel).GenerateJools,
            minimum:=StartingWealthLevels.Descriptors(embarkSettings.StartingWealthLevel).MinimumJools)
    End Sub
End Class
