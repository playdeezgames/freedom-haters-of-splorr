Public Class AvatarStatusModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        Should.Throw(Of NullReferenceException)(Function() sut.GameOver)
        Should.Throw(Of NullReferenceException)(Function() sut.Bankrupt)
        Should.Throw(Of NullReferenceException)(Function() sut.Dead)
    End Sub

    Private Function CreateSut() As IAvatarStatusModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Avatar.Status
    End Function
End Class
