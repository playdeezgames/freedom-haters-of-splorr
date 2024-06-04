Public Class StartingWealthLevelModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Current.ShouldNotBeNull
        sut.CurrentName.ShouldNotBeNull
        sut.Options.Count.ShouldBe(5)
    End Sub
    <Fact>
    Sub set_wealth_level()
        Dim sut = CreateSut()
        Dim item = sut.Options.First.Item
        sut.SetWealthLevel(item)
        sut.Current.ShouldBe(item)
    End Sub

    Private Function CreateSut() As IStartingWealthLevelModel
        Return CreateOneStepUniverse(
            AddressOf BuildLonelyUniverse,
            writeStringToFile:=AddressOf WriteFile,
            readStringFromFile:=AddressOf ReadFile).Settings.StartingWealth
    End Function

    Private Function ReadFile(arg As String) As String
        Throw New NotImplementedException()
    End Function

    Private Sub WriteFile(arg1 As String, arg2 As String)
        'do nothing
    End Sub
End Class
