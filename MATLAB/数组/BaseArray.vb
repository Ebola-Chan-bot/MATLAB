Public MustInherit Class BaseArray
	Private Shared Function 排除小尾数(尾数 As Integer, ParamArray 索引 As Integer()) As Integer()
		Dim a As SByte
		For a = 索引.GetUpperBound(0) To 0 Step -1
			If 索引(a) > 尾数 Then Exit For
		Next
		a = Math.Max(a, 0)
		ReDim Preserve 索引(a)
		Return 索引
	End Function
	Friend 各维长度 As Integer()
	MustOverride ReadOnly Property NumEl As Integer
	Public ReadOnly Property NDims As Byte
		Get
			Return 各维长度.Length
		End Get
	End Property

	MustOverride Function Clone() As BaseArray
	Sub New(各维长度 As Integer())
		Reshape(各维长度)
	End Sub
	''' <summary>
	''' 不同于<see cref="ElMat.Reshape(BaseArray, Integer?())"/>，这个方法不返回新数组，直接对原数组进行修改
	''' </summary>
	Public Sub Reshape(ParamArray sz As Integer())
		各维长度 = 排除小尾数(1, sz)
	End Sub

	Public Function Size() As IEnumerable(Of Integer)
		Return 各维长度
	End Function
	Public Function Size(维度 As Byte) As Integer
		If 维度 < NDims Then
			Return 各维长度(维度)
		Else
			Return 1
		End If
	End Function
End Class
