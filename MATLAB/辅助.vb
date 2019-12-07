Friend Module 辅助
	Sub 适配递归(原数组 As Array, 总维数 As Byte, 新数组 As Array, 当前索引 As Integer(), 当前维数 As Byte)
		If 当前维数 < 总维数 - 1 Then
			For a As Integer = 0 To 原数组.GetUpperBound(当前维数)
				当前索引(当前维数) = a
				适配递归(原数组, 总维数, 新数组, 当前索引, 当前维数 + 1)
			Next
		Else
			For a As Integer = 0 To 原数组.GetUpperBound(当前维数)
				当前索引(当前维数) = a
				新数组.SetValue(原数组.GetValue(当前索引), 当前索引)
			Next
		End If
	End Sub
	Sub 适配递归(原数组1 As Array, 原数组2 As Array, 各维长度 As Integer(), 总维数 As Byte, 新数组1 As Array, 新数组2 As Array, 当前索引 As Integer(), 当前维数 As Byte)
		Dim b As Integer = 原数组1.GetLength(当前维数), c As Integer = 原数组2.GetLength(当前维数)
		If 当前维数 < 总维数 - 1 Then
			If b < c Then
				For a As Integer = 0 To b - 1
					当前索引(当前维数) = a
					适配递归(原数组1, 原数组2, 各维长度, 总维数, 新数组1, 新数组2, 当前索引, 当前维数 + 1)
				Next
				For a As Integer = b To c - 1
					当前索引(当前维数) = a
					适配递归(原数组2， 总维数, 新数组2, 当前索引, 当前维数 + 1)
				Next
			Else
				For a As Integer = 0 To c - 1
					当前索引(当前维数) = a
					适配递归(原数组1, 原数组2, 各维长度, 总维数, 新数组1, 新数组2, 当前索引, 当前维数 + 1)
				Next
				For a As Integer = c To b - 1
					当前索引(当前维数) = a
					适配递归(原数组1， 总维数, 新数组1, 当前索引, 当前维数 + 1)
				Next
			End If
		Else
			If b < c Then
				For a As Integer = 0 To b - 1
					新数组1.SetValue(原数组1.GetValue(当前索引), 当前索引)
					新数组2.SetValue(原数组2.GetValue(当前索引), 当前索引)
				Next
				For a As Integer = b To c - 1
					新数组2.SetValue(原数组2.GetValue(当前索引), 当前索引)
				Next
			Else
				For a As Integer = 0 To c - 1
					新数组1.SetValue(原数组1.GetValue(当前索引), 当前索引)
					新数组2.SetValue(原数组1.GetValue(当前索引), 当前索引)
				Next
				For a As Integer = c To b - 1
					新数组1.SetValue(原数组1.GetValue(当前索引), 当前索引)
				Next
			End If
		End If
	End Sub
	Function 适配(ByRef 数组1 As Array, ByRef 数组2 As Array) As Integer()
		Dim b As Byte = Math.Max(数组1.Rank, 数组2.Rank), c(b - 1) As Integer, f As Boolean = False, g As Boolean = False, h As Integer, i As Integer
		For a As Byte = 0 To b - 1
			h = 数组1.GetLength(a)
			i = 数组2.GetLength(a)
			If h = i Then
				c(a) = h
			ElseIf h < i Then
				f = True
				c(a) = i
			Else
				g = True
				c(a) = h
			End If
		Next
		If f AndAlso g Then
			Dim d As Array = Array.CreateInstance(数组1.Class, c), e As Array = Array.CreateInstance(数组2.Class, c)
			适配递归(数组1, 数组2, c, b, d, e, Zeros(Of Integer)(b), 0)
			数组1 = d
			数组2 = e
		Else
			If f Then
				Dim d As Array = Array.CreateInstance(数组1.Class, c)
				适配递归(数组1, b, d, Zeros(Of Integer)(b), 0)
				数组1 = d
			End If
			If g Then
				Dim d As Array = Array.CreateInstance(数组1.Class, c)
				适配递归(数组2, b, d, Zeros(Of Integer)(b), 0)
				数组2 = d
			End If
		End If
		Return c
	End Function
End Module
