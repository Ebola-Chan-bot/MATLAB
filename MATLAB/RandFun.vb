Public Module RandFun
	''' <summary>
	''' 返回由随机数组成的 sz1×...×szN 数组，其中 sz1,...,szN 指示每个维度的大小。例如：<c>Rand(3, 4)</c>返回一个 3×4 的矩阵。
	''' </summary>
	''' <param name="sz">每个维度的大小</param>
	''' <returns>指定尺寸的随机数组</returns>
	Public Function Rand(ParamArray sz As UInteger()) As Array(Of Double)
		Rand = New Array(Of Double)(sz)
		Static 随机生成器 As New Random
		For a = 0 To Rand.Numel - 1
			Rand.本体(a) = 随机生成器.NextDouble
		Next
	End Function
End Module
