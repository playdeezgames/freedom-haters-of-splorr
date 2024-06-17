Public MustInherit Class EntityData_should(Of TEntityData As IEntityData)
    Protected MustOverride Function CreateSut() As TEntityData
    <Fact>
    Sub default_given_flag_to_false()
        Dim sut = CreateSut()
        Const flagName = "flag name"
        sut.HasFlag(flagName).ShouldBeFalse
    End Sub
    <Fact>
    Sub set_flag_to_true()
        Dim sut = CreateSut()
        Const flagName = "flag name"
        sut.SetFlag(flagName)
        sut.HasFlag(flagName).ShouldBeTrue
    End Sub
    <Theory>
    <InlineData(" ")>
    <InlineData("  ")>
    <InlineData("")>
    <InlineData(Nothing)>
    Sub disallow_invalid_flags(flagName As String)
        Dim sut = CreateSut()
        Should.Throw(Of InvalidOperationException)(Sub() sut.SetFlag(flagName))
    End Sub
    <Fact>
    Sub clear_flag()
        Dim sut = CreateSut()
        Const flagName = "flag name"
        sut.SetFlag(flagName)
        sut.HasFlag(flagName).ShouldBeTrue
        sut.ClearFlag(flagName)
        sut.HasFlag(flagName).ShouldBeFalse
    End Sub
End Class
