﻿Imports SPLORR.UI

Friend Class FHOSUIPalette
    Implements IUIPalette

    Public ReadOnly Property Background As Integer Implements IUIPalette.Background
        Get
            Return 3
        End Get
    End Property

    Public ReadOnly Property MenuItem As Integer Implements IUIPalette.MenuItem
        Get
            Return 0
        End Get
    End Property

    Public ReadOnly Property Header As Integer Implements IUIPalette.Header
        Get
            Return 7
        End Get
    End Property

    Public ReadOnly Property Footer As Integer Implements IUIPalette.Footer
        Get
            Return 8
        End Get
    End Property

    Public ReadOnly Property [Error] As Integer Implements IUIPalette.Error
        Get
            Return 4
        End Get
    End Property
End Class
