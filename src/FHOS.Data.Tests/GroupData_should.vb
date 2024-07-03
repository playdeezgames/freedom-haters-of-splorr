Public Class GroupData_should
    Inherits IdentifiedEntityData_should(Of GroupData)
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
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
    <Fact>
    Sub add_child()
        Dim sut = CreateSut()
        Const childId = 1
        sut.AddChild(childId)
        sut.HasChildren.ShouldBeTrue
        sut.AllChildren.ShouldHaveSingleItem
        sut.HasChild(childId).ShouldBeTrue
    End Sub
    <Fact>
    Sub remove_child()
        Dim sut = CreateSut()
        Const childId = 1
        sut.AddChild(childId)
        sut.RemoveChild(childId)
        sut.HasChildren.ShouldBeFalse
        sut.AllChildren.ShouldBeEmpty
        sut.HasChild(childId).ShouldBeFalse
    End Sub
    <Fact>
    Sub add_parent()
        Dim sut = CreateSut()
        Const parentId = 1
        sut.AddParent(parentId)
        sut.HasParents.ShouldBeTrue
        sut.AllParents.ShouldHaveSingleItem
        sut.HasParent(parentId).ShouldBeTrue
    End Sub
    <Fact>
    Sub remove_parent()
        Dim sut = CreateSut()
        Const parentId = 1
        sut.AddParent(parentId)
        sut.RemoveParent(parentId)
        sut.HasParents.ShouldBeFalse
        sut.AllParents.ShouldBeEmpty
        sut.HasParent(parentId).ShouldBeFalse
    End Sub
    Protected Overrides Function CreateSut() As GroupData
        Return New GroupData With {.Id = 0}
    End Function
End Class
