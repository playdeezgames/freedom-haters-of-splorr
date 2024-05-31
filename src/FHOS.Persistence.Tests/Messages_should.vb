Imports System.Net.Http

Public Class Messages_should
    <Fact>
    Public Sub have_default_values_upon_instantiation()
        Dim sut = CreateMessages()
        sut.HasAny.ShouldBeFalse
        Should.Throw(Of InvalidOperationException)(Function() sut.Current)
        Should.Throw(Of InvalidOperationException)(Sub() sut.Dismiss())
        Should.NotThrow(Sub() sut.Add("test"))
    End Sub
    <Fact>
    Public Sub add_and_dismiss_message()
        Dim sut = CreateMessages()

        sut.HasAny.ShouldBeFalse

        Const header = "test"

        sut.Add(header)
        sut.HasAny.ShouldBeTrue
        sut.Current.ShouldNotBeNull
        sut.Current.Header.ShouldBe(header)

        sut.Dismiss()
        sut.HasAny.ShouldBeFalse
        Should.Throw(Of InvalidOperationException)(Function() sut.Current)
    End Sub
End Class
