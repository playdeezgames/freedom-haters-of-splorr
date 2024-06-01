Imports System.Runtime.CompilerServices

Public Class Store_should
    <Fact>
    Sub have_an_id()
        Dim sut As IStore = CreateSut()
        sut.Id.ShouldBe(0)
    End Sub

    Private Shared Function CreateSut(
                                     Optional value As Integer = 0,
                                     Optional minimum As Integer? = Nothing,
                                     Optional maximum As Integer? = Nothing) As IStore
        Return CreateStore(
            value,
            minimum,
            maximum)
    End Function
    <Theory>
    <InlineData(0, 1, 1)>
    <InlineData(0, 0, 0)>
    <InlineData(0, -1, 0)>
    Sub maintain_minimum_value(minimum As Integer, value As Integer, expected As Integer)
        Dim sut = CreateSut(minimum, minimum)
        sut.CurrentValue = value
        sut.CurrentValue.ShouldBe(expected)
    End Sub
    <Theory>
    <InlineData(0, 1, 0)>
    <InlineData(0, 0, 0)>
    <InlineData(0, -1, -1)>
    Sub maintain_maximum_value(maximum As Integer, value As Integer, expected As Integer)
        Dim sut = CreateSut(maximum, Nothing, maximum)
        sut.CurrentValue = value
        sut.CurrentValue.ShouldBe(expected)
    End Sub
    <Fact>
    Sub not_allow_max_less_than_min()
        Should.Throw(Of ArgumentOutOfRangeException)(Function() CreateSut(0, 1, -1))
    End Sub
End Class
