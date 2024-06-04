Public Class AvatarVerbsModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Available.Count.ShouldBe(2)
        sut.HasAny.ShouldBeTrue
        Should.Throw(Of ArgumentNullException)(Sub() sut.Perform(Nothing))
    End Sub

    Private Function CreateSut() As IAvatarVerbsModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Avatar.Verbs
    End Function
End Class
