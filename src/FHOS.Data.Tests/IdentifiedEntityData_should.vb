Public MustInherit Class IdentifiedEntityData_should(Of TEntityData As IIdentifiedEntityData)
    Inherits EntityData_should(Of TEntityData)
    <Fact>
    Sub have_id()
        Dim sut = CreateSut()
        sut.Id.ShouldBe(0)
    End Sub
End Class
