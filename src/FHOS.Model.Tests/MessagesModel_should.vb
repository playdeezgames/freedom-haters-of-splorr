Public Class MessagesModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.HasAny.ShouldBeFalse()
        Should.Throw(Of InvalidOperationException)(Function() sut.Current)
        Should.Throw(Of InvalidOperationException)(Sub() sut.Dismiss())
    End Sub

    Private Function CreateSut() As IMessagesModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Messages
    End Function
End Class
