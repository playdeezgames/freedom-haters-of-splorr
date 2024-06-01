Imports System.Security.Cryptography.X509Certificates

Public Class ActorFamily_should
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
                              Optional data As UniverseData = Nothing) As IActorFamily
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
            data).Family
    End Function
    <Fact>
    Sub have_default_values_when_instantiated()
        Dim sut = CreateSut()
        sut.Parent.ShouldBeNull
        sut.HasChildren.ShouldBeFalse
        sut.Children.ShouldBeEmpty
        Should.Throw(Of NullReferenceException)(Sub() sut.AddChild(Nothing))
    End Sub
    <Fact>
    Sub add_child()
        Dim universe = CreateUniverse()
        Dim sut = CreateSut(universe:=universe)
        Dim child = CreateActor("other map name", "other map type", 3, 2, "other location type", 1, 1, "other actor type", "other actor name", universe:=universe)
        sut.AddChild(child)
        sut.HasChildren.ShouldBeTrue
        sut.Children.Count.ShouldBe(1)
        sut.Children.Single.Id.ShouldBe(child.Id)
    End Sub
    <Fact>
    Sub set_parent()
        Dim universe = CreateUniverse()
        Dim sut = CreateSut(universe:=universe)
        Dim parent = CreateActor("other map name", "other map type", 3, 2, "other location type", 1, 1, "other actor type", "other actor name", universe:=universe)
        sut.Parent = parent
        sut.Parent.Id.ShouldBe(parent.Id)
    End Sub
End Class
