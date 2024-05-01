Friend Class Star
    Inherits Place
    Implements IStar

    Public Sub New(universeData As Data.UniverseData, placeId As Integer)
        MyBase.New(universeData, placeId)
    End Sub
End Class
