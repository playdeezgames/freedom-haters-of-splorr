Public Class AvatarStateModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Position.X.ShouldBe(0)
        sut.Position.Y.ShouldBe(0)
        sut.MapName.ShouldBe("map name")
    End Sub

    Private Function CreateSut() As IAvatarStateModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Avatar.State
    End Function
End Class
