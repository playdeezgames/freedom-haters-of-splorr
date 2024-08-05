Public Interface IItemModel
    ReadOnly Property DisplayName As String
    ReadOnly Property Description As String
    ReadOnly Property InstallFee As Integer
    ReadOnly Property UniqueName As String
    ReadOnly Property CanUse As Boolean
    Sub Use(actor As IActorModel)
End Interface
