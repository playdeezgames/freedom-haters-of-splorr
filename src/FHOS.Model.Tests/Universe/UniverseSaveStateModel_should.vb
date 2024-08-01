Public Class UniverseSaveStateModel_should
    Private ReadOnly files As New Dictionary(Of String, String)
    Private Function CreateSut(initialize As Boolean) As IUniverseSaveStateModel
        Dim sut = New UniverseModel(
            AddressOf WriteStringToFile,
            AddressOf ReadStringFromFile,
            generationTimeSlice:=0.0,
            initializer:=New EmptyUniverseInitializer(0))
        If initialize Then
            sut.Generator.Start()
            While Not sut.Generator.Done
                sut.Generator.Generate()
            End While
        End If
        Return sut.SaveState
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
        Dim sut = CreateSut(False)
        Should.NotThrow(Sub() sut.Save(saveFilename))
        Should.NotThrow(Sub() sut.Load(saveFilename))
        Should.NotThrow(Sub() sut.Abandon())
        files.Count.ShouldBe(1)
        files(saveFilename).ShouldBe("null")
    End Sub

    Const DefaultSavedData = "{""Actors"":{},""Locations"":{},""Maps"":{},""Groups"":{},""Stores"":{},""Items"":{},""Avatar"":null,""NextActorId"":0,""NextLocationId"":0,""NextMapId"":0,""NextGroupId"":0,""NextStoreId"":0,""NextItemId"":0,""Flags"":[],""Statistics"":{""Turn"":1},""Metadatas"":{}}"

    <Fact>
    Sub save_current_universe()
        Const saveFilename = "save.json"
        Dim sut = CreateSut(True)
        sut.Save(saveFilename)
        files.Count.ShouldBe(1)
        files(saveFilename).ShouldBe(DefaultSavedData)
    End Sub

    <Fact>
    Sub load_current_universe()
        Const loadFilename = "load.json"
        files(loadFilename) = DefaultSavedData
        Dim sut = CreateSut(True)
        sut.Load(loadFilename)
        Const saveFilename = "save.json"
        sut.Save(saveFilename)
        files.Count.ShouldBe(2)
        files(saveFilename).ShouldBe(files(loadFilename))
    End Sub

    <Fact>
    Sub abandon_current_universe()
        Const loadFilename = "load.json"
        files(loadFilename) = DefaultSavedData
        Dim sut = CreateSut(True)
        sut.Load(loadFilename)
        Const saveFilename = "save.json"
        sut.Abandon()
        sut.Save(saveFilename)
        files.Count.ShouldBe(2)
        files(saveFilename).ShouldBe("null")
    End Sub
End Class
