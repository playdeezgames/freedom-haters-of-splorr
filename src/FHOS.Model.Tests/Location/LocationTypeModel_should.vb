Imports System.Data.Common

Public Class LocationTypeModel_should
    <Fact>
    Sub have_initial_values_upon_initialization()
        Dim sut = CreateSut()
        Should.Throw(Of KeyNotFoundException)(Function() sut.Name)
        Should.Throw(Of KeyNotFoundException)(Function() sut.Sprite)
    End Sub

    Private Function CreateSut() As ILocationTypeModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.GetLocation((0, 0)).LocationType
    End Function
End Class
