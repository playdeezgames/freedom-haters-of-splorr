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
        Const saveFilename = "save.json"
        Dim sut = CreateSut()
        sut.Generator.ShouldNotBeNull
        sut.Settings.ShouldNotBeNull
        sut.State.ShouldNotBeNull
        sut.Pedia.ShouldNotBeNull
        sut.Ephemerals.ShouldNotBeNull
        Should.NotThrow(Sub() sut.Save(saveFilename))
        Should.NotThrow(Sub() sut.Load(saveFilename))
        Should.NotThrow(Sub() sut.Abandon())
        files.Count.ShouldBe(1)
        files(saveFilename).ShouldBe("null")
        ValidateEphemerals(sut)
    End Sub

    Const DefaultSavedData = "{""Actors"":{},""Locations"":{},""Maps"":{},""Groups"":{},""Stores"":{},""Items"":{},""Avatars"":[],""NextActorId"":0,""NextLocationId"":0,""NextMapId"":0,""NextGroupId"":0,""NextStoreId"":0,""NextItemId"":0,""Flags"":[],""Statistics"":{""Turn"":1},""Metadatas"":{}}"

    <Fact>
    Sub save_current_universe()
        Const saveFilename = "save.json"
        Dim sut = CreateSut()
        sut.Generator.Start()
        While Not sut.Generator.Done
            sut.Generator.Generate()
        End While
        sut.Save(saveFilename)
        files.Count.ShouldBe(1)
        files(saveFilename).ShouldBe(DefaultSavedData)
    End Sub

    <Fact>
    Sub load_current_universe()
        Const loadFilename = "load.json"
        files(loadFilename) = DefaultSavedData
        Dim sut = CreateSut()
        PushToEphemerals(sut)
        sut.Load(loadFilename)
        ValidateEphemerals(sut)
        Const saveFilename = "save.json"
        sut.Save(saveFilename)
        files.Count.ShouldBe(2)
        files(saveFilename).ShouldBe(files(loadFilename))
    End Sub

    Private Shared Sub ValidateEphemerals(sut As IUniverseModel)
        sut.Ephemerals.SelectedFaction.ShouldBeEmpty
        sut.Ephemerals.SelectedPlanet.ShouldBeEmpty
        sut.Ephemerals.SelectedSatellite.ShouldBeEmpty
        sut.Ephemerals.SelectedStarSystem.ShouldBeEmpty
        sut.Ephemerals.CurrentOffer.ShouldBeNull
        sut.Ephemerals.CurrentPrice.ShouldBeNull
    End Sub

    Private Shared Sub PushToEphemerals(sut As IUniverseModel)
        sut.Ephemerals.SelectedFaction.Push(Nothing)
        sut.Ephemerals.SelectedPlanet.Push(Nothing)
        sut.Ephemerals.SelectedSatellite.Push(Nothing)
        sut.Ephemerals.SelectedStarSystem.Push(Nothing)
    End Sub

    <Fact>
    Sub abandon_current_universe()
        Const loadFilename = "load.json"
        files(loadFilename) = DefaultSavedData
        Dim sut = CreateSut()
        sut.Load(loadFilename)
        Const saveFilename = "save.json"
        PushToEphemerals(sut)
        sut.Abandon()
        ValidateEphemerals(sut)
        sut.Save(saveFilename)
        files.Count.ShouldBe(2)
        files(saveFilename).ShouldBe("null")
    End Sub
End Class
