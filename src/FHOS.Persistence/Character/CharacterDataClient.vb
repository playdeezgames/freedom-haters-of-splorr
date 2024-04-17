Imports FHOS.Data

Friend Class CharacterDataClient
    Inherits WorldDataClient
    Sub New(worldData As UniverseData, characterId As Integer)
        MyBase.New(worldData)
        Me.CharacterId = characterId
    End Sub
    Protected ReadOnly Property CharacterId As Integer
    Protected ReadOnly Property CharacterData As ActorData
        Get
            Return WorldData.Actors(CharacterId)
        End Get
    End Property
End Class
