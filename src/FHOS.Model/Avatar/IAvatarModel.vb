Public Interface IAvatarModel
    ReadOnly Property Bio As IAvatarBioModel
    ReadOnly Property Verbs As IAvatarVerbsModel
    ReadOnly Property KnownPlaces As IAvatarKnownPlacesModel
    ReadOnly Property Tutorial As IAvatarTutorialModel
    ReadOnly Property Stack As IAvatarStackModel
    ReadOnly Property Status As IAvatarStatusModel

    ReadOnly Property Position As (X As Integer, Y As Integer)
    ReadOnly Property MapName As String
    ReadOnly Property CurrentPlace As IPlaceModel

    ReadOnly Property IsGameOver As Boolean
    ReadOnly Property IsDead As Boolean
    ReadOnly Property IsBankrupt As Boolean

    'Vessel
    ReadOnly Property AvailableCrew As IEnumerable(Of (Name As String, Actor As IActorModel))
    ReadOnly Property OxygenPercent As Integer
    ReadOnly Property OxygenHue As Integer
    ReadOnly Property FuelPercent As Integer
    ReadOnly Property FuelHue As Integer

    ReadOnly Property Turn As Integer
    ReadOnly Property Jools As Integer
    ReadOnly Property MinimumJools As Integer

    Sub LeaveInteraction()
    ReadOnly Property IsInteracting As Boolean
End Interface
