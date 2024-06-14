Imports FHOS.Persistence

Friend Class AvatarInitializationStep
    Inherits InitializationStep
    ReadOnly universe As IUniverse
    ReadOnly embarkSettings As EmbarkSettings
    Sub New(universe As IUniverse, embarkSettings As EmbarkSettings)
        Me.universe = universe
        Me.embarkSettings = embarkSettings
    End Sub
    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim actor = universe.Avatar.Actor
        actor.Yokes.YokedStore(YokeTypes.Wallet) = universe.Factory.CreateStore(
            StartingWealthLevels.Descriptors(embarkSettings.StartingWealthLevel).GenerateJools,
            minimum:=StartingWealthLevels.Descriptors(embarkSettings.StartingWealthLevel).MinimumJools)
        For Each crewMember In actor.Family.Children
            crewMember.Yokes.YokedStore(YokeTypes.Wallet) = actor.Yokes.YokedStore(YokeTypes.Wallet)
        Next
    End Sub
End Class
