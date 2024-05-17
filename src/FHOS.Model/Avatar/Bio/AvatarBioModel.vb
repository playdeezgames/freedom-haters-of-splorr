Imports FHOS.Persistence

Friend Class AvatarBioModel
    Inherits BaseAvatarModel
    Implements IAvatarBioModel

    Protected Sub New(actor As IActor)
        MyBase.New(actor)
    End Sub

    Public ReadOnly Property HomePlanet As IPlaceModel Implements IAvatarBioModel.HomePlanet
        Get
            Return PlaceModel.FromPlace(actor.Properties.HomePlanet)
        End Get
    End Property

    Public ReadOnly Property Faction As IFactionModel Implements IAvatarBioModel.Faction
        Get
            Return FactionModel.FromFaction(actor.Properties.Faction)
        End Get
    End Property

    Friend Shared Function FromActor(actor As IActor) As IAvatarBioModel
        Return New AvatarBioModel(actor)
    End Function
End Class
