Public Class ItemData_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut As IItemData = New ItemData
        sut.Flags.ShouldBeEmpty
        sut.Statistics.ShouldBeEmpty
        sut.Metadatas.ShouldBeEmpty
    End Sub
    <Fact>
    Sub default_given_flag_to_false()
        Dim sut As IItemData = New ItemData
        Const flagName = "flag name"
        sut.HasFlag(flagName).ShouldBeFalse
        sut.Flags.Contains(flagName).ShouldBeFalse
    End Sub
    <Fact>
    Sub set_flag_to_true()
        Dim sut As IItemData = New ItemData
        Const flagName = "flag name"
        sut.SetFlag(flagName)
        sut.HasFlag(flagName).ShouldBeTrue
        sut.Flags.Contains(flagName).ShouldBeTrue
    End Sub
    <Theory>
    <InlineData(" ")>
    <InlineData("  ")>
    <InlineData("")>
    <InlineData(Nothing)>
    Sub disallow_invalid_flags(flagName As String)
        Dim sut As IItemData = New ItemData
        Should.Throw(Of InvalidOperationException)(Sub() sut.SetFlag(flagName))
    End Sub
End Class

