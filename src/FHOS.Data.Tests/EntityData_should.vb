Public MustInherit Class EntityData_should(Of TEntityData As IEntityData)
    Protected MustOverride Function CreateSut() As TEntityData
    <Fact>
    Sub default_given_flag_to_false()
        Dim sut = CreateSut()
        Const flagType = "flag type"
        sut.HasFlag(flagType).ShouldBeFalse
    End Sub
    <Fact>
    Sub set_flag_to_true()
        Dim sut = CreateSut()
        Const flagType = "flag type"
        sut.SetFlag(flagType)
        sut.HasFlag(flagType).ShouldBeTrue
    End Sub
    <Theory>
    <InlineData(" ")>
    <InlineData("  ")>
    <InlineData("")>
    <InlineData(Nothing)>
    Sub disallow_invalid_flags(flagType As String)
        Dim sut = CreateSut()
        Should.Throw(Of InvalidOperationException)(Sub() sut.SetFlag(flagType))
        Should.Throw(Of InvalidOperationException)(Sub() sut.HasFlag(flagType))
    End Sub
    <Fact>
    Sub clear_flag()
        Dim sut = CreateSut()
        Const flagType = "flag type"
        sut.SetFlag(flagType)
        sut.HasFlag(flagType).ShouldBeTrue
        sut.ClearFlag(flagType)
        sut.HasFlag(flagType).ShouldBeFalse
    End Sub
    <Fact>
    Sub default_given_statistic_to_nothing()
        Dim sut = CreateSut()
        Const statisticType = "statistic type"
        sut.GetStatistic(statisticType).ShouldBeNull
    End Sub
    <Fact>
    Sub set_statistic()
        Dim sut = CreateSut()
        Const statisticType = "statistic type"
        Const statisticValue = 1
        sut.SetStatistic(statisticType, statisticValue)
        sut.GetStatistic(statisticType).ShouldBe(statisticValue)
    End Sub
    <Fact>
    Sub clear_statistic()
        Dim sut = CreateSut()
        Const statisticType = "statistic type"
        Const statisticValue = 1
        sut.SetStatistic(statisticType, statisticValue)
        sut.SetStatistic(statisticType, Nothing)
        sut.GetStatistic(statisticType).ShouldBeNull
    End Sub
    <Theory>
    <InlineData(" ")>
    <InlineData("  ")>
    <InlineData("")>
    <InlineData(Nothing)>
    Sub disallow_invalid_statistics(statisticType As String)
        Dim sut = CreateSut()
        Const statisticValue = 1
        Should.Throw(Of InvalidOperationException)(Sub() sut.SetStatistic(statisticType, statisticValue))
        Should.Throw(Of InvalidOperationException)(Sub() sut.GetStatistic(statisticType))
    End Sub
End Class
