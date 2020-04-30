Public Class ByteArray
	Inherits TypedArray(Of Byte)
	Private Shared Function 生成原型(各维长度 As Integer()) As Byte()
		Dim a(各维长度.Aggregate(Function(b As Integer, c As Integer) b * c) - 1) As Byte
		Return a
	End Function
	Sub New(各维长度 As Integer())
		MyBase.New(各维长度, 生成原型(各维长度))
	End Sub
	Sub New(各维长度 As Integer(), 原型 As Byte())
		MyBase.New(各维长度, 原型)
	End Sub
	Sub New(原型 As Byte())
		MyBase.New({原型.Length}, 原型)
	End Sub
	Public Overrides Function Clone() As BaseArray
		Return New ByteArray(各维长度.Clone, 本体.ToArray)
	End Function
End Class
