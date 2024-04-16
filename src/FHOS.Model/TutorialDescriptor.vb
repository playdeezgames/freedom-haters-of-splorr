Friend Class TutorialDescriptor
    Public ReadOnly Property HasIgnoreFlag As Boolean
        Get
            Return Not String.IsNullOrEmpty(IgnoreFlag)
        End Get
    End Property
    Public ReadOnly Property IgnoreFlag As String
    Sub New(Optional ignoreFlag As String = Nothing)
        Me.IgnoreFlag = ignoreFlag
    End Sub
End Class
