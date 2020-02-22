Public Structure MInt8
	Implements INumeric
	Private 数值 As SByte
	Shared ReadOnly MinValue As New MInt8(SByte.MinValue), MaxValue As New MInt8(SByte.MaxValue)
	Sub New(数值 As SByte)
		Me.数值 = 数值
	End Sub
	Sub New(数值 As INumeric)
		If 数值.Lt(MinValue) Then
			Me.数值 = SByte.MinValue
		ElseIf Double.IsNaN(数值.RawData) Then
			Me.数值 = 0
		ElseIf 数值.Gt(MaxValue) Then
			Me.数值 = SByte.MaxValue
		Else
			Me.数值 = 数值.RawData
		End If
	End Sub
	Public ReadOnly Property RawData As Object Implements INumeric.RawData
		Get
			Return 数值
		End Get
	End Property

	Shared Widening Operator CType(数值 As SByte) As MInt8
		Return New MInt8(数值)
	End Operator

	Shared Widening Operator CType(数值 As MInt8) As SByte
		Return 数值.数值
	End Operator

	Shared Widening Operator CType(数值 As MInt8) As MDecimal
		Return 数值.数值
	End Operator

	Shared Widening Operator CType(数值 As MInt8) As MSingle
		Return 数值.数值
	End Operator

	Shared Widening Operator CType(数值 As MInt8) As MInt16
		Return 数值.数值
	End Operator

	Shared Widening Operator CType(数值 As MInt8) As MInt64
		Return 数值.数值
	End Operator

	Shared Widening Operator CType(数值 As MInt8) As MInt32
		Return 数值.数值
	End Operator

	Shared Widening Operator CType(数值 As MInt8) As MDouble
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MInt8) As MUInt8
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MInt8) As MUInt16
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MInt8) As MUInt32
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MInt8) As MUInt64
		Return 数值.数值
	End Operator

	Shared Operator +(A As MInt8, B As INumeric) As MInt8
		Return A.数值 + CSByte(B.RawData)
	End Operator

	Shared Operator -(A As MInt8, B As INumeric) As MInt8
		Return A.数值 - CSByte(B.RawData)
	End Operator

	Shared Operator *(A As MInt8, B As INumeric) As MInt8
		Return A.数值 * CSByte(B.RawData)
	End Operator

	Shared Operator /(A As MInt8, B As INumeric) As MInt8
		Return A.数值 / CSByte(B.RawData)
	End Operator

	Shared Operator >(A As MInt8, B As INumeric) As Boolean
		Return A.数值 > CSByte(B.RawData)
	End Operator

	Shared Operator <(A As MInt8, B As INumeric) As Boolean
		Return A.数值 < CSByte(B.RawData)
	End Operator

	Public Function Plus(B As INumeric) As INumeric Implements INumeric.Plus
		Return Me + B
	End Function

	Public Function Minus(B As INumeric) As INumeric Implements INumeric.Minus
		Return Me - B
	End Function

	Public Function Times(B As INumeric) As INumeric Implements INumeric.Times
		Return Me * B
	End Function

	Public Function RDivide(B As INumeric) As INumeric Implements INumeric.RDivide
		Return Me / B
	End Function

	Public Function Gt(B As INumeric) As Boolean Implements INumeric.Gt
		Return Me > B
	End Function

	Public Function Lt(B As INumeric) As Boolean Implements INumeric.Lt
		Return Me < B
	End Function

	Public Function Min(B As INumeric) As INumeric Implements INumeric.Min
		Return New MInt8(Math.Min(数值, CSByte(B.RawData)))
	End Function

	Public Function Max(B As INumeric) As INumeric Implements INumeric.Max
		Return New MInt8(Math.Max(数值, CSByte(B.RawData)))
	End Function

	Public Overrides Function Equals(obj As Object) As Boolean
		If obj.GetType.GetInterface("INumeric") Is Nothing Then
			Return 数值.Equals(CSByte(obj))
		Else
			Return 数值.Equals(CSByte(DirectCast(obj, INumeric).RawData))
		End If
	End Function

	Public Shared Operator =(left As MInt8, right As INumeric) As Boolean
		Return left.数值.Equals(CSByte(right.RawData))
	End Operator

	Public Shared Operator <>(left As MInt8, right As INumeric) As Boolean
		Return left.数值.Equals(CSByte(right.RawData))
	End Operator

	Public Sub SetValue(value As INumeric) Implements INumeric.SetValue
		数值 = value.RawData
	End Sub

	Public Sub SetValue(value As Object) Implements INumeric.SetValue
		数值 = value
	End Sub
	Shared Function CastFrom(ParamArray 数组 As SByte()) As MInt8()
		Return 数组.AsParallel.AsOrdered.Select(Function(a As SByte) New MInt8(a)).ToArray
	End Function
	Shared Function CastBack(ParamArray 数组 As MInt8()) As SByte()
		Return 数组.AsParallel.AsOrdered.Select(Function(a As MInt8) a.数值).ToArray
	End Function
	Public Overrides Function ToString() As String
		Return 数值
	End Function
End Structure
