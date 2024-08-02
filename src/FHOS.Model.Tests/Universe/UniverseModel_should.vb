Public Class UniverseModel_should
    Private ReadOnly files As New Dictionary(Of String, String)
    Private Function CreateSut() As IUniverseModel
        Return New UniverseModel(
            AddressOf WriteStringToFile,
            AddressOf ReadStringFromFile,
            generationTimeSlice:=0.0,
            initializer:=New EmptyUniverseInitializer(0))
    End Function

    Private Function ReadStringFromFile(filename As String) As String
        Return files(filename)
    End Function

    Private Sub WriteStringToFile(filename As String, contents As String)
        files(filename) = contents
    End Sub

    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Generator.ShouldNotBeNull
        sut.Settings.ShouldNotBeNull
        sut.State.ShouldNotBeNull
        sut.Pedia.ShouldNotBeNull
        sut.SaveState.ShouldNotBeNull
        files.Count.ShouldBe(0)
    End Sub
End Class
