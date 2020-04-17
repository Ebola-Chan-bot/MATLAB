Public Class ByteArray
	Inherits TypedArray(Of Byte)
	Sub New(各维长度 As Integer())
		MyBase.New(各维长度)
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
