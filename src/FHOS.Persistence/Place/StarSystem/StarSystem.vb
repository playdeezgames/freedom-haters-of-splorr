Imports FHOS.Data

Friend Class StarSystem
    Inherits Place
    Implements IStarSystem

    Public Sub New(universeData As Data.UniverseData, starSystemId As Integer)
        MyBase.New(universeData, starSystemId)
    End Sub
End Class
