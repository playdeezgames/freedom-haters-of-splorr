Imports FHOS.Persistence

Friend Class StarSystemModel
    Inherits PlaceModel
    Implements IStarSystemModel

    Private ReadOnly starSystem As IStarSystem

    Public Sub New(starSystem As IStarSystem)
        MyBase.New(starSystem)
        Me.starSystem = starSystem
    End Sub
End Class
