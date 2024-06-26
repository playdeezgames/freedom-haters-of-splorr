﻿Public Class Group_should
    <Fact>
    Sub have_an_id()
        Dim sut As IGroup = CreateSut()
        sut.Id.ShouldBe(0)
    End Sub

    Private Shared Function CreateSut() As IGroup
        Const groupType = "group type"
        Const groupName = "group name"
        Return CreateGroup(groupType, groupName)
    End Function

    <Fact>
    Sub have_flags()
        Dim sut = CreateSut()
        Const flagName = "flag name"
        sut.Flags(flagName).ShouldBeFalse
        sut.Flags(flagName) = True
        sut.Flags(flagName).ShouldBeTrue
        sut.Flags(flagName) = False
        sut.Flags(flagName).ShouldBeFalse
    End Sub
    <Fact>
    Sub set_name()
        Const entityName = "entity name"
        Dim sut = CreateSut()
        sut.EntityName = entityName
        sut.EntityName.ShouldBe(entityName)
    End Sub
    <Fact>
    Sub have_no_metadata()
        Const metadataType = "metadata type"
        Dim sut = CreateSut()
        sut.Metadatas(metadataType).ShouldBeNull
    End Sub

    <Fact>
    Sub set_metadata()
        Const metadataType = "metadata type"
        Const metadataValue = "metadata value"
        Dim sut = CreateSut()
        sut.Metadatas(metadataType) = metadataValue
        sut.Metadatas(metadataType).ShouldBe(metadataValue)
    End Sub
    <Fact>
    Sub have_universe_reference()
        Dim sut = CreateSut()
        sut.Universe.ShouldNotBeNull
        'prove that the universe contains the sut
        sut.Universe.Groups.Count.ShouldBe(1)
        sut.Universe.Groups.First().Id.ShouldBe(sut.Id)
    End Sub
    <Fact>
    Sub have_statistics()
        Dim sut = CreateSut()
        Const statisticType = "statistic type"
        sut.Statistics(statisticType).ShouldBeNull
        Const statisticValue = 10
        sut.Statistics(statisticType) = statisticValue
        sut.Statistics(statisticType).ShouldBe(statisticValue)
    End Sub
End Class
