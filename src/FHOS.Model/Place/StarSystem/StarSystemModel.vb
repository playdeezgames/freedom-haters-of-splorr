Imports FHOS.Persistence

Friend Class StarSystemModel
    Inherits PlaceModel
    Implements IStarSystemModel

    Private ReadOnly starSystem As IStarSystem

    Public Sub New(starSystem As IStarSystem)
        Me.starSystem = starSystem
    End Sub

    Public ReadOnly Property Name As String Implements IStarSystemModel.Name
        Get
            Return starSystem.Name
        End Get
    End Property
End Class
