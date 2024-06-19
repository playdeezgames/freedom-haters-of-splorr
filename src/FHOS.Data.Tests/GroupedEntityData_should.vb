Public MustInherit Class GroupedEntityData_should(Of TEntityData As IGroupedEntityData)
    Inherits EntityData_should(Of TEntityData)
    <Fact>
    Sub default_given_yoked_group_to_nothing()
        Dim sut = CreateSut()
        Const yokeType = "yoke type"
        sut.GetYokedGroup(yokeType).ShouldBeNull
    End Sub
    <Fact>
    Sub set_yoked_group()
        Dim sut = CreateSut()
        Const yokeType = "yoke type"
        Const groupId = 1
        sut.SetYokedGroup(yokeType, groupId)
        sut.GetYokedGroup(yokeType).ShouldBe(groupId)
    End Sub
    <Fact>
    Sub clear_yoked_group()
        Dim sut = CreateSut()
        Const yokeType = "yoke type"
        Const groupId = 1
        sut.SetYokedGroup(yokeType, groupId)
        sut.SetYokedGroup(yokeType, Nothing)
        sut.GetYokedGroup(yokeType).ShouldBeNull
    End Sub
    <Theory>
    <InlineData(" ")>
    <InlineData("  ")>
    <InlineData("")>
    <InlineData(Nothing)>
    Sub disallow_invalid_group_yoke_types(yokeType As String)
        Dim sut = CreateSut()
        Should.Throw(Of InvalidOperationException)(Function() sut.GetYokedGroup(yokeType))
        Should.Throw(Of InvalidOperationException)(Sub() sut.SetYokedGroup(yokeType, 1))
    End Sub
End Class
