Public Class AvatarTutorialModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        Should.Throw(Of ArgumentNullException)(Sub() sut.Dismiss())
        sut.HasAny.ShouldBeFalse
        sut.Current.ShouldBeNull
        sut.IgnoreCurrent.ShouldBeTrue
    End Sub

    Private Function CreateSut() As IAvatarTutorialModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Avatar.Tutorial
    End Function
End Class
