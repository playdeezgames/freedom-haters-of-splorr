Public Class Avatar_should
    <Fact>
    Sub have_default_values_upon_instantiation()
        Dim data As New UniverseData
        Dim universe As New Universe(data)
        Dim sut = universe.Avatar
        sut.Actor.ShouldBeNull
        Should.Throw(Of InvalidOperationException)(Sub() sut.Pop())
        Should.Throw(Of NullReferenceException)(Sub() sut.Push(Nothing))
    End Sub
End Class
