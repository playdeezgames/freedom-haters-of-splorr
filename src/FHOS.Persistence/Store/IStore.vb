Public Interface IStore
    Inherits IEntity
    Property CurrentValue As Integer
    ReadOnly Property MaximumValue As Integer?
    ReadOnly Property MinimumValue As Integer?
End Interface
