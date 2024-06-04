Public Class AvatarStackModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        Should.Throw(Of NullReferenceException)(Sub() sut.Push(Nothing))
        Should.NotThrow(Sub() sut.Pop())
    End Sub

    Private Shared Function CreateSut() As IAvatarStackModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Avatar.Stack
    End Function
End Class
