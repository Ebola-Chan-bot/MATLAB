Public Interface IArray
	Inherits IEnumerable
	Function Size() As UInteger()
	Function Size([dim] As Byte) As UInteger
	ReadOnly Property NDims As Byte
	Function 循环割补(ParamArray 尺寸 As UInteger()) As IArray
	Function GetValue(ParamArray index As UInteger()) As Object
End Interface
