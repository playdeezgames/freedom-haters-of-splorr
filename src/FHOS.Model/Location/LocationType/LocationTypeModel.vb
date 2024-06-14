Friend Class LocationTypeModel
    Implements ILocationTypeModel

    Private ReadOnly location As Persistence.ILocation

    Public Sub New(location As Persistence.ILocation)
        Me.location = location
    End Sub

    Public ReadOnly Property Name As String Implements ILocationTypeModel.Name
        Get
            Return LocationTypes.Descriptors(location.EntityType).Name
        End Get
    End Property

    Public ReadOnly Property Sprite As (Glyph As Char, Foreground As Integer, Background As Integer) Implements ILocationTypeModel.Sprite
        Get
            With LocationTypes.Descriptors(location.EntityType)
                Return (.Glyph, .Foreground, .Background)
            End With
        End Get
    End Property
End Class
