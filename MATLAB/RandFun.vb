Public Module RandFun
	Private Sub Rand递归(数组 As Array, 当前维度 As Byte, 当前索引 As Integer())
		Static 随机生成器 As New Random
		If 当前维度 < 数组.Rank - 1 Then
			For a As Integer = 0 To 数组.GetUpperBound(当前维度)
				当前索引(当前维度) = a
				Rand递归(数组, 当前维度 + 1, 当前索引)
			Next
		Else
			For a As Integer = 0 To 数组.GetUpperBound(当前维度)
				当前索引(当前维度) = a
				数组.SetValue(随机生成器.NextDouble, 当前索引)
			Next
		End If
	End Sub
	''' <summary>
	''' (0,1)均匀分布的随机数
	''' </summary>
	''' <param name="sz">每个维度的大小</param>
	''' <returns>指定尺寸的随机数组</returns>
	Public Function Rand(ParamArray sz As Integer()) As Array(Of Double)
		Rand = New Array(Of Double)(sz)
		Rand递归(Rand, 0, Zeros(Of Integer)(sz.Length))
	End Function
End Module
