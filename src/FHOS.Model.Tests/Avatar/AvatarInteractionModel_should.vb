Public Class AvatarInteractionModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        Should.NotThrow(Sub() sut.Leave())
        sut.IsActive.ShouldBeFalse
        Should.Throw(Of NullReferenceException)(Function() sut.AvailableChoices)
        Should.Throw(Of NullReferenceException)(Function() sut.Lines)
    End Sub

    Private Function CreateSut() As IAvatarInteractionModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Avatar.Yokes.Interaction
    End Function
End Class
