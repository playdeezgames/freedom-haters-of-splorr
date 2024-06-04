Public Class GalacticAgeModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Current.ShouldNotBeNull
        sut.CurrentName.ShouldNotBeNull
        sut.Options.Count.ShouldBe(3)
    End Sub
    <Fact>
    Sub set_age()
        Dim sut = CreateSut()
        Dim item = sut.Options.First.Item
        sut.SetAge(item)
        sut.Current.ShouldBe(item)
    End Sub

    Private Function CreateSut() As IGalacticAgeModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).Settings.GalacticAge
    End Function
End Class
