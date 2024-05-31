Public Class UniverseFactory_should
    <Fact>
    Sub create_group()
        Dim data As New UniverseData
        Dim universe As New Universe(data)
        Dim sut = universe.Factory
        Const groupName = "group name"
        Const groupType = "group type"
        Dim group = sut.CreateGroup(groupType, groupName)
        group.ShouldNotBeNull
        group.GroupType.ShouldBe(groupType)
        group.Name.ShouldBe(groupName)
        universe.Groups.Count.ShouldBe(1)
        group.LegacyGroup.ShouldBeNull
        Should.Throw(Of InvalidOperationException)(Function() group.MinimumPlanetCount)
        group.PlanetCount.ShouldBe(0)
        group.SatelliteCount.ShouldBe(0)
        Should.Throw(Of InvalidOperationException)(Function() group.Authority)
        Should.Throw(Of InvalidOperationException)(Function() group.Standards)
        Should.Throw(Of InvalidOperationException)(Function() group.Conviction)
        group.Parents.ShouldBeEmpty
        group.Children.ShouldBeEmpty
        Should.Throw(Of NullReferenceException)(Sub() group.AddParent(Nothing))
        Should.NotThrow(Sub() group.RemoveParent(Nothing))
    End Sub
End Class
