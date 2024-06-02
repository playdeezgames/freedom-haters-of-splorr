Public Class UniverseModel_should
    Private ReadOnly files As New Dictionary(Of String, String)
    Private Function CreateSut() As IUniverseModel
        Return New UniverseModel(AddressOf WriteStringToFile, AddressOf ReadStringFromFile)
    End Function

    Private Function ReadStringFromFile(filename As String) As String
        Return files(filename)
    End Function

    Private Sub WriteStringToFile(filename As String, contents As String)
        files(filename) = contents
    End Sub

    <Fact>
    Sub have_default_values_upon_initialization()
        Const saveFilename = "save.json"
        Dim sut = CreateSut()
        sut.Generator.ShouldNotBeNull
        sut.Settings.ShouldNotBeNull
        sut.State.ShouldNotBeNull
        sut.Pedia.ShouldNotBeNull
        Should.NotThrow(Sub() sut.Save(saveFilename))
        Should.NotThrow(Sub() sut.Load(saveFilename))
        Should.NotThrow(Sub() sut.Abandon())
        files.Count.ShouldBe(1)
        files(saveFilename).ShouldBe("null")
    End Sub
End Class


