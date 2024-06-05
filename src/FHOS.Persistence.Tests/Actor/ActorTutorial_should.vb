Public Class ActorTutorial_should
    Private Function CreateSut(
                              Optional mapName As String = "map name",
                              Optional mapType As String = "map type",
                              Optional mapColumns As Integer = 3,
                              Optional mapRows As Integer = 4,
                              Optional locationType As String = "location type",
                              Optional column As Integer = 0,
                              Optional row As Integer = 0,
                              Optional actorType As String = "actor type",
                              Optional actorName As String = "actor name",
                              Optional universe As IUniverse = Nothing,
                              Optional data As UniverseData = Nothing) As IActorTutorial
        Return CreateActor(
            mapName,
            mapType,
            mapColumns,
            mapRows,
            locationType,
            column,
            row,
            actorType,
            actorName,
            universe,
            data).Tutorial
    End Function
    <Fact>
    Sub have_default_values_upon_instantiation()
        Dim sut = CreateSut()
        sut.HasAny.ShouldBeFalse
        sut.Current.ShouldBeNull
        Should.NotThrow(Sub() sut.Dismiss())
        Should.NotThrow(Sub() sut.Add(Nothing))
    End Sub
    <Fact>
    Sub ignore_when_null_tutorial_added()
        Dim sut = CreateSut()
        sut.Add(Nothing)
        sut.HasAny.ShouldBeFalse
    End Sub
    <Fact>
    Sub know_when_current_tutorial_present()
        Const tutorial = "tutorial"
        Dim sut = CreateSut()
        sut.Add(tutorial)
        sut.HasAny.ShouldBeTrue
        sut.Current.ShouldBe(tutorial)
    End Sub
    <Fact>
    Sub dismiss_correctly()
        Const tutorial = "tutorial"
        Dim sut = CreateSut()
        sut.Add(tutorial)
        sut.Dismiss()
        sut.HasAny.ShouldBeFalse
        sut.Current.ShouldBe(Nothing)
    End Sub
End Class
