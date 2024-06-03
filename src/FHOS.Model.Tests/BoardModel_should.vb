Public Class BoardModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.GetLocation((0, 0)).ShouldNotBeNull
    End Sub

    Private Function CreateSut() As IBoardModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Board
    End Function
End Class
