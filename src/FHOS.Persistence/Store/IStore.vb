Public Interface IStore
    Inherits IEntity
    Property CurrentValue As Integer
    Property MaximumValue As Integer?
    ReadOnly Property MinimumValue As Integer?
    ReadOnly Property Percent As Integer?
End Interface
