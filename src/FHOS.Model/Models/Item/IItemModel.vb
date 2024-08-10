Public Interface IItemModel
    ReadOnly Property DisplayName As String
    ReadOnly Property Description As IEnumerable(Of String)
    ReadOnly Property InstallFee As Integer
    ReadOnly Property UniqueName As String
End Interface
