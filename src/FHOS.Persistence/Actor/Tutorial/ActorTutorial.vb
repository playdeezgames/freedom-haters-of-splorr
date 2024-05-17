Friend Class ActorTutorial
    Inherits ActorDataClient
    Implements IActorTutorial

    Protected Sub New(universeData As Data.UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Friend Shared Function FromId(universeData As Data.UniverseData, id As Integer) As IActorTutorial
        Return New ActorTutorial(universeData, id)
    End Function
    Public Sub Add(tutorial As String) Implements IActorTutorial.Add
        If tutorial Is Nothing Then
            Return
        End If
        EntityData.Tutorials.Enqueue(tutorial)
    End Sub

    Public Sub Dismiss() Implements IActorTutorial.Dismiss
        If HasAny Then
            EntityData.Tutorials.Dequeue()
        End If
    End Sub
    Public ReadOnly Property HasAny As Boolean Implements IActorTutorial.HasAny
        Get
            Return EntityData.Tutorials.Any
        End Get
    End Property

    Public ReadOnly Property Current As String Implements IActorTutorial.Current
        Get
            If HasAny Then
                Return EntityData.Tutorials.Peek
            End If
            Return Nothing
        End Get
    End Property
End Class
