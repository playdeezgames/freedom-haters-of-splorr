Public Interface IPlace
    Inherits IEntity
    ReadOnly Property PlaceType As String
    ReadOnly Property Subtype As String
    ReadOnly Property Family As IPlaceFamily
    ReadOnly Property Properties As IPlaceProperties
    ReadOnly Property Factory As IPlaceFactory
End Interface
