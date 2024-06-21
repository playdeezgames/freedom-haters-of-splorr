Public MustInherit Class IdentifiedEntityData_should(Of TEntityData As IIdentifiedEntityData)
    Inherits EntityData_should(Of TEntityData)
    Private ReadOnly tablePrefix As String
    Sub New(tablePrefix As String)
        Me.tablePrefix = tablePrefix
    End Sub
    <Fact>
    Sub have_id()
        Dim sut = CreateSut()
        sut.Id.ShouldBe(0)
    End Sub
    Protected Overrides Sub VerifyFlagClear(entityData As TEntityData, flagType As String)
        '        Using command = Connection.CreateCommand
        '            command.CommandText = $"
        'SELECT 
        '    COUNT(1) 
        'FROM 
        '    [{tablePrefix}Flags] 
        'WHERE 
        '    [FlagType]=@FlagType AND 
        '    [{tablePrefix}Id]=@{tablePrefix}Id;"
        '            command.Parameters.AddWithValue("FlagType", flagType)
        '            command.Parameters.AddWithValue($"{tablePrefix}Id", entityData.Id)
        '            CInt(command.ExecuteScalar).ShouldBe(0)
        '        End Using
    End Sub
End Class
