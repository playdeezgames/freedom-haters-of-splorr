Public Interface IAvatarStateModel
    ReadOnly Property Position As (X As Integer, Y As Integer)
    ReadOnly Property MapName As String
    ReadOnly Property CurrentPlace As IPlaceModel
    ReadOnly Property Scrap As Integer
End Interface
