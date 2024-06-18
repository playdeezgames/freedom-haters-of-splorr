Public Class GroupData_should
    Inherits EntityData_should(Of IGroupData)
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Children.ShouldBeEmpty
        sut.Parents.ShouldBeEmpty
        sut.HasChildren.ShouldBeFalse
        sut.HasParents.ShouldBeFalse
        sut.AllChildren.ShouldBeEmpty
        sut.AllParents.ShouldBeEmpty
    End Sub
    <Fact>
    Sub by_default_not_have_given_child()
        Dim sut = CreateSut()
        sut.HasChild(1).ShouldBeFalse
    End Sub
    <Fact>
    Sub by_default_not_have_given_parent()
        Dim sut = CreateSut()
        sut.HasParent(1).ShouldBeFalse
    End Sub
    Protected Overrides Function CreateSut() As IGroupData
        Return New GroupData
    End Function
End Class
