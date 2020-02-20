Public Interface IArray
	Inherits IEnumerable
	Function Size() As UInteger()
	Function Size([dim] As Byte) As UInteger
	ReadOnly Property NDims As Byte
	Function 循环割补(ParamArray 尺寸 As UInteger()) As IArray
	Function GetValue(ParamArray index As UInteger()) As Object
	Function [Decimal]() As Array(Of MDecimal)
	Function [Double]() As Array(Of MDouble)
	Function Int16() As Array(Of MInt16)
	Function Int32() As Array(Of MInt32)
	Function Int64() As Array(Of MInt64)
	Function Int8() As Array(Of MInt8)
	Function [Single]() As Array(Of MSingle)
	Function UInt16() As Array(Of MUInt16)
	Function UInt32() As Array(Of MUInt32)
	Function UInt64() As Array(Of MUInt64)
	Function UInt8() As Array(Of MUInt8)
	Function NDecimal() As Array(Of Decimal)
	Function NDouble() As Array(Of Double)
	Function NSingle() As Array(Of Single)
	Function NSByte() As Array(Of SByte)
	Function NShort() As Array(Of Short)
	Function NInteger() As Array(Of Integer)
	Function NLong() As Array(Of Long)
	Function NByte() As Array(Of Byte)
	Function NUShort() As Array(Of UShort)
	Function NUInteger() As Array(Of UInteger)
	Function NULong() As Array(Of ULong)
End Interface
