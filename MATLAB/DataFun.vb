Public Module DataFun
	''' <summary>
	''' 查找 A 的所有元素的最小值。
	''' </summary>
	''' <param name="A">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	''' <returns>最小值标量</returns>
	Public Function Min(Of T As INumeric)(A As Array(Of T)) As T
		Return A.Min()
	End Function
	''' <summary>
	''' 计算向量 vecdim 所指定的维度上的最小值。例如，如果 A 是矩阵，则<c>Min(A, 0, 1)</c>计算 A 中所有元素的最小值，因为矩阵的每个元素都包含在由维度 0 和 1 定义的数组切片中。
	''' </summary>
	''' <param name="A">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	''' <param name="vecdim">维度向量，指定为正整数向量。每个元素代表输入数组的一个维度。指定的操作维度的输出长度为 1，而其他保持不变。</param>
	''' <returns>最小值数组</returns>
	Public Function Min(Of T As INumeric)(A As Array(Of T), ParamArray vecdim As Byte()) As Array(Of T)
		Return A.Min(vecdim)
	End Function
	''' <summary>
	''' 返回从 A 或 B 中提取的最小元素的数组。
	''' </summary>
	''' <param name="A">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	''' <param name="B">其他输入数组，指定为标量、向量、矩阵或多维数组</param>
	''' <returns>A 或 B 中的最小元素，以标量、向量、矩阵或多维数组的形式返回。C 的大小由 A 和 B 的维度的隐式扩展决定</returns>
	Public Function Min(Of T As INumeric)(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Return BsxFun(Function(c As T, d As T) DirectCast(c.Min(d), T), A, B)
	End Function
	''' <summary>
	''' 查找 A 的所有元素的最大值。
	''' </summary>
	''' <param name="A">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	''' <returns>最大值标量</returns>
	Public Function Max(Of T As INumeric)(A As Array(Of T)) As T
		Return A.Max()
	End Function
	''' <summary>
	''' 计算向量 vecdim 所指定的维度上的最大值。例如，如果 A 是矩阵，则<c>Max(A, 0, 1)</c>计算 A 中所有元素的最大值，因为矩阵的每个元素都包含在由维度 0 和 1 定义的数组切片中。
	''' </summary>
	''' <param name="A">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	''' <param name="vecdim">维度向量，指定为正整数向量。每个元素代表输入数组的一个维度。指定的操作维度的输出长度为 1，而其他保持不变。</param>
	''' <returns>最大值数组</returns>
	Public Function Max(Of T As INumeric)(A As Array(Of T), ParamArray vecdim As Byte()) As Array(Of T)
		Return A.Max(vecdim)
	End Function
	''' <summary>
	''' 返回从 A 或 B 中提取的最大元素的数组。
	''' </summary>
	''' <param name="A">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	''' <param name="B">其他输入数组，指定为标量、向量、矩阵或多维数组</param>
	''' <returns>A 或 B 中的最大元素，以标量、向量、矩阵或多维数组的形式返回。C 的大小由 A 和 B 的维度的隐式扩展决定</returns>
	Public Function Max(Of T As INumeric)(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Return BsxFun(Function(c As T, d As T) DirectCast(c.Max(d), T), A, B)
	End Function
End Module
