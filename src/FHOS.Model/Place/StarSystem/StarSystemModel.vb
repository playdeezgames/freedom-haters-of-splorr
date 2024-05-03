Imports FHOS.Persistence

Friend Class StarSystemModel
    Inherits PlaceModel
    Implements IStarSystemModel

    Private ReadOnly starSystem As IPlace

    Public Sub New(starSystem As IPlace)
        MyBase.New(starSystem)
        Me.starSystem = starSystem
    End Sub
End Class
