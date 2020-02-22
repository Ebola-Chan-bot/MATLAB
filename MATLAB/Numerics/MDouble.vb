Public Structure MDouble
	Implements INumeric
	Private 数值 As Double
	Shared ReadOnly Property NaN As MDouble = New MDouble(Double.NaN)
	Sub New(数值 As Double)
		Me.数值 = 数值
	End Sub
	Sub New(数值 As INumeric)
		Me.数值 = 数值.RawData
	End Sub
	Public ReadOnly Property RawData As Object Implements INumeric.RawData
		Get
			Return 数值
		End Get
	End Property

	Shared Widening Operator CType(数值 As Double) As MDouble
		Return New MDouble(数值)
	End Operator

	Shared Widening Operator CType(数值 As MDouble) As Double
		Return 数值.数值
	End Operator

	Shared Widening Operator CType(数值 As MDouble) As MDecimal
		If 数值.数值 < Decimal.MinValue Then Return Decimal.MinValue
		If Double.IsNaN(数值.数值) Then Return 0
		If 数值.数值 > Decimal.MaxValue Then Return Decimal.MaxValue
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MDouble) As MSingle
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MDouble) As MInt8
		If 数值.数值 < SByte.MinValue Then Return SByte.MinValue
		If Double.IsNaN(数值.数值) Then Return 0
		If 数值.数值 > SByte.MaxValue Then Return SByte.MaxValue
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MDouble) As MInt16
		If 数值.数值 < Short.MinValue Then Return Short.MinValue
		If Double.IsNaN(数值.数值) Then Return 0
		If 数值.数值 > Short.MaxValue Then Return Short.MaxValue
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MDouble) As MInt32
		If 数值.数值 < Integer.MinValue Then Return Integer.MinValue
		If Double.IsNaN(数值.数值) Then Return 0
		If 数值.数值 > Integer.MaxValue Then Return Integer.MaxValue
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MDouble) As MInt64
		If 数值.数值 < Long.MinValue Then Return Long.MinValue
		If Double.IsNaN(数值.数值) Then Return 0
		If 数值.数值 > Long.MaxValue Then Return Long.MaxValue
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MDouble) As MUInt8
		If 数值.数值 < Byte.MinValue Then Return Byte.MinValue
		If Double.IsNaN(数值.数值) Then Return 0
		If 数值.数值 > Byte.MaxValue Then Return Byte.MaxValue
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MDouble) As MUInt16
		If 数值.数值 < UShort.MinValue Then Return UShort.MinValue
		If Double.IsNaN(数值.数值) Then Return 0
		If 数值.数值 > UShort.MaxValue Then Return UShort.MaxValue
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MDouble) As MUInt32
		If 数值.数值 < UInteger.MinValue Then Return UInteger.MinValue
		If Double.IsNaN(数值.数值) Then Return 0
		If 数值.数值 > UInteger.MaxValue Then Return UInteger.MaxValue
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MDouble) As MUInt64
		If 数值.数值 < ULong.MinValue Then Return ULong.MinValue
		If Double.IsNaN(数值.数值) Then Return 0
		If 数值.数值 > ULong.MaxValue Then Return ULong.MaxValue
		Return 数值.数值
	End Operator

	Shared Operator +(A As MDouble, B As INumeric) As MDouble
		Return A.数值 + CDbl(B.RawData)
	End Operator

	Shared Operator -(A As MDouble, B As INumeric) As MDouble
		Return A.数值 - CDbl(B.RawData)
	End Operator

	Shared Operator *(A As MDouble, B As INumeric) As MDouble
		Return A.数值 * CDbl(B.RawData)
	End Operator

	Shared Operator /(A As MDouble, B As INumeric) As MDouble
		Return A.数值 / CDbl(B.RawData)
	End Operator

	Shared Operator >(A As MDouble, B As INumeric) As Boolean
		Return A.数值 > CDbl(B.RawData)
	End Operator

	Shared Operator <(A As MDouble, B As INumeric) As Boolean
		Return A.数值 < CDbl(B.RawData)
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
		If Double.IsNaN(数值) Then Return B
		If Double.IsNaN(B.RawData) Then Return Me
		Return New MDouble(Math.Min(数值, B.RawData))
	End Function

	Public Function Max(B As INumeric) As INumeric Implements INumeric.Max
		If Double.IsNaN(数值) Then Return B
		If Double.IsNaN(B.RawData) Then Return Me
		Return New MDouble(Math.Max(数值, B.RawData))
	End Function

	Public Overrides Function Equals(obj As Object) As Boolean
		If obj.GetType.GetInterface("INumeric") Is Nothing Then
			Return 数值.Equals(CDbl(obj))
		Else
			Return 数值.Equals(CDbl(DirectCast(obj, INumeric).RawData))
		End If
	End Function

	Public Shared Operator =(left As MDouble, right As INumeric) As Boolean
		Return left.数值.Equals(CDbl(right.RawData))
	End Operator

	Public Shared Operator <>(left As MDouble, right As INumeric) As Boolean
		Return Not left.数值.Equals(CDbl(right.RawData))
	End Operator

	Public Sub SetValue(value As INumeric) Implements INumeric.SetValue
		数值 = value.RawData
	End Sub

	Public Sub SetValue(value As Object) Implements INumeric.SetValue
		数值 = value
	End Sub
	Shared Function CastFrom(ParamArray 数组 As Double()) As MDouble()
		Return 数组.AsParallel.AsOrdered.Select(Function(a As Double) New MDouble(a)).ToArray
	End Function
	Shared Function CastBack(ParamArray 数组 As MDouble()) As Double()
		Return 数组.AsParallel.AsOrdered.Select(Function(a As MDouble) a.数值).ToArray
	End Function
	Public Overrides Function ToString() As String
		Return 数值
	End Function
End Structure
