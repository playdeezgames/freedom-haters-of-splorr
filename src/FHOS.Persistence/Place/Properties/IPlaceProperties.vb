Public Interface IPlaceProperties
    Property Name As String
    Property Map As IMap
    ReadOnly Property Identifier As String
    Property Faction As IFaction
    ReadOnly Property Position As (X As Integer, Y As Integer)
End Interface
