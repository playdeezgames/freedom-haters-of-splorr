Imports System.Runtime.CompilerServices
Imports FHOS.Persistence

Friend Module ItemExtensions
    <Extension>
    Friend Function Descriptor(item As IItem) As ItemTypeDescriptor
        Return ItemTypes.Descriptors(item.EntityType)
    End Function
End Module
