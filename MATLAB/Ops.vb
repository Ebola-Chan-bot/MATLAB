Public Module Ops
	Public ReadOnly [End] As MDouble = MDouble.NaN
	''' <summary>
	''' 创建一个固定间隔向量 x，以 i 作为元素之间的增量。向量元素大致为 [j,j+i,j+2*i,...,j+m*i]，其中 m = fix((k-j)/i)。但是，如果 i 不是整数，则浮点算术运算的作用在于确定 colon 是否包括向量中的端点 k，因为 k 可能并不是正好等于 j+m*i。
	''' </summary>
	''' <param name="j">起点</param>
	''' <param name="i">公差</param>
	''' <param name="k">终点</param>
	''' <returns>等差数列</returns>
	Public Function Colon(j As INumeric, i As INumeric, k As INumeric) As ColonExpression
		Return New ColonExpression(j, i, k)
	End Function
	''' <summary>
	''' 创建一个包含元素 [j,j+1,j+2,...,j+m] 的单位间距向量 x，其中 m = fix(k-j)。如果 j 和 k 都是整数，则简化为 [j,j+1,...,k]。
	''' </summary>
	''' <param name="j">起点</param>
	''' <param name="k">终点</param>
	''' <returns>等差数列</returns>
	Public Function Colon(j As Integer, k As Integer?) As IntegerColon
		Return New IntegerColon(j, 1, k)
	End Function
	''' <summary>
	''' 创建一个包含元素 [j,j+1,j+2,...,j+m] 的单位间距向量 x，其中 m = fix(k-j)。如果 j 和 k 都是整数，则简化为 [j,j+1,...,k]。
	''' </summary>
	''' <param name="j">起点</param>
	''' <param name="k">终点</param>
	''' <returns>等差数列</returns>
	Public Function Colon(j As INumeric, k As INumeric) As ColonExpression
		Return New ColonExpression(j, k)
	End Function
	''' <summary>
	''' 创建一个固定间隔向量 x，以 i 作为元素之间的增量。向量元素大致为 [j,j+i,j+2*i,...,j+m*i]，其中 m = fix((k-j)/i)。但是，如果 i 不是整数，则浮点算术运算的作用在于确定 colon 是否包括向量中的端点 k，因为 k 可能并不是正好等于 j+m*i。
	''' </summary>
	''' <typeparam name="T">返回值类型</typeparam>
	''' <param name="j">起点</param>
	''' <param name="i">公差</param>
	''' <param name="k">终点</param>
	''' <returns>指定类型的等差数列</returns>
	Public Function Colon(j As Double, i As Double, k As Double) As ColonExpression
		Return New ColonExpression(j, i, k)
	End Function
	''' <summary>
	''' 创建一个包含元素 [j,j+1,j+2,...,j+m] 的单位间距向量 x，其中 m = fix(k-j)。如果 j 和 k 都是整数，则简化为 [j,j+1,...,k]。
	''' </summary>
	''' <typeparam name="T">返回值类型</typeparam>
	''' <param name="j">起点</param>
	''' <param name="k">终点</param>
	''' <returns>指定类型的等差数列</returns>
	Public Function Colon(j As Double, k As Double) As ColonExpression
		Return New ColonExpression(j, k)
	End Function
	Public Function Index(Of T As INumeric)(数值 As T) As UInteger
		Return 数值.RawData
	End Function
	Public Function Index(数组 As IArray) As UInteger()
		Return 数组.NUInteger
	End Function
	Public Function Index(数组 As Array) As UInteger()
		If 数组.Class.GetInterface("INumeric") Is Nothing Then
			Return (From a In 数组 Select CUInt(a)).ToArray
		Else
			Return (From a As INumeric In 数组 Select CUInt(a.RawData)).ToArray
		End If
	End Function
	''' <summary>
	''' 数组加法，每个元素的位置对应相加产生新数组，如果尺寸不匹配则循环填充扩展。数组元素必须实现<see cref="INumeric"/>
	''' </summary>
	''' <typeparam name="T">返回元素类型</typeparam>
	''' <param name="A">被加数组</param>
	''' <param name="B">加数组</param>
	''' <returns>和数组</returns>
	Public Function Plus(Of T As INumeric)(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Return A + B
	End Function
	''' <summary>
	''' 数组数加法，每个元素加上常数得到新数组，数组元素必须实现<see cref="INumeric"/>
	''' </summary>
	''' <typeparam name="T">返回元素类型</typeparam>
	''' <param name="A">被加数组</param>
	''' <param name="B">加常数</param>
	''' <returns>和数组</returns>
	Public Function Plus(Of T As INumeric)(A As Array(Of T), B As T) As Array(Of T)
		Return A + B
	End Function
	''' <summary>
	''' 数组减法，每个元素的位置对应相减产生新数组，如果尺寸不匹配则循环填充扩展。数组元素必须实现<see cref="INumeric"/>
	''' </summary>
	''' <typeparam name="T">返回元素类型</typeparam>
	''' <param name="A">被减数组</param>
	''' <param name="B">减数组</param>
	''' <returns>差数组</returns>
	Public Function Minus(Of T As INumeric)(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Return A - B
	End Function
	''' <summary>
	''' 数组数减法，每个元素减去常数得到新数组，数组元素必须实现<see cref="INumeric"/>
	''' </summary>
	''' <typeparam name="T">返回元素类型</typeparam>
	''' <param name="A">被减数组</param>
	''' <param name="B">减常数</param>
	''' <returns>差数组</returns>
	Public Function Minus(Of T As INumeric)(A As Array(Of T), B As T) As Array(Of T)
		Return A - B
	End Function
	''' <summary>
	''' 数组点乘法，每个元素的位置对应相乘产生新数组，如果尺寸不匹配则循环填充扩展。数组元素必须实现<see cref="INumeric"/>
	''' </summary>
	''' <typeparam name="T">返回元素类型</typeparam>
	''' <param name="A">乘数组1</param>
	''' <param name="B">乘数组2</param>
	''' <returns>积数组</returns>
	Public Function Times(Of T As INumeric)(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Return A * B
	End Function
	''' <summary>
	''' 数组数乘法，每个元素乘上常数得到新数组，数组元素必须实现<see cref="INumeric"/>
	''' </summary>
	''' <typeparam name="T">返回元素类型</typeparam>
	''' <param name="A">乘数组</param>
	''' <param name="B">乘常数</param>
	''' <returns>积数组</returns>
	Public Function Times(Of T As INumeric)(A As Array(Of T), B As T) As Array(Of T)
		Return A * B
	End Function
	''' <summary>
	'''数组右除,每个元素的位置对应相除产生新数组，如果尺寸不匹配则循环填充扩展。数组元素必须实现<see cref="INumeric"/>
	''' </summary>
	''' <typeparam name="T">返回元素类型</typeparam>
	''' <param name="A">被除数组</param>
	''' <param name="B">除数组</param>
	''' <returns>商数组</returns>
	Public Function RDivide(Of T As INumeric)(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Return A / B
	End Function
	''' <summary>
	''' 数组数除法，每个元素除去常数得到新数组，数组元素必须实现<see cref="INumeric"/>
	''' </summary>
	''' <typeparam name="T">返回元素类型</typeparam>
	''' <param name="A">被除数组</param>
	''' <param name="B">除常数</param>
	''' <returns>商数组</returns>
	Public Function RDivide(Of T As INumeric)(A As Array(Of T), B As T) As Array(Of T)
		Return A / B
	End Function
	''' <summary>
	''' 确定相等性。对两个数组对应位置判断是否相等，返回<see cref="Boolean"/>结果存于新数组的对应位置。数组元素必须为基本数值类型或定义了=运算符。
	''' </summary>
	''' <typeparam name="T">比较数据类型</typeparam>
	''' <param name="A">左数组</param>
	''' <param name="B">右数组</param>
	''' <returns>比较结果数组</returns>
	Public Function Eq(Of T)(A As Array(Of T), B As Array(Of T)) As Array(Of Boolean)
		Return A = B
	End Function
	''' <summary>
	''' 确定相等性，每个元素判断是否等于常数，返回<see cref="Boolean"/>结果存于新数组的对应位置。数组元素必须为基本数值类型或定义了=运算符
	''' </summary>
	''' <typeparam name="T">比较数据类型</typeparam>
	''' <param name="A">左数组</param>
	''' <param name="B">右常数</param>
	''' <returns>比较结果数组</returns>
	Public Function Eq(Of T)(A As Array(Of T), B As T) As Array(Of Boolean)
		Return A = B
	End Function
	''' <summary>
	''' 确定不相等性。对两个数组对应位置判断是否不相等，返回<see cref="Boolean"/>结果存于新数组的对应位置。数组元素必须为基本数值类型或定义了&lt;&gt;运算符
	''' </summary>
	''' <typeparam name="T">比较数据类型</typeparam>
	''' <param name="A">左数组</param>
	''' <param name="B">右数组</param>
	''' <returns>比较结果数组</returns>
	Public Function Ne(Of T)(A As Array(Of T), B As Array(Of T)) As Array(Of Boolean)
		Return A <> B
	End Function
	''' <summary>
	''' 确定不相等性，每个元素判断是否不等于常数，返回<see cref="Boolean"/>结果存于新数组的对应位置。数组元素必须为基本数值类型或定义了&lt;&gt;运算符
	''' </summary>
	''' <typeparam name="T">比较数据类型</typeparam>
	''' <param name="A">左数组</param>
	''' <param name="B">右常数</param>
	''' <returns>比较结果数组</returns>
	Public Function Ne(Of T)(A As Array(Of T), B As T) As Array(Of Boolean)
		Return A <> B
	End Function
	''' <summary>
	''' 确定大于。对两个数组对应位置判断左边是否大于右边，返回<see cref="Boolean"/>结果存于新数组的对应位置。数组元素必须实现<see cref="INumeric"/>
	''' </summary>
	''' <typeparam name="T">比较数据类型</typeparam>
	''' <param name="A">左数组</param>
	''' <param name="B">右数组</param>
	''' <returns>比较结果数组</returns>
	Public Function Gt(Of T As INumeric)(A As Array(Of T), B As Array(Of T)) As Array(Of Boolean)
		Return A > B
	End Function
	''' <summary>
	''' 确定大于。每个元素判断是否大于常数，返回<see cref="Boolean"/>结果存于新数组的对应位置。数组元素必须实现<see cref="INumeric"/>
	''' </summary>
	''' <typeparam name="T">比较数据类型</typeparam>
	''' <param name="A">左数组</param>
	''' <param name="B">右常数</param>
	''' <returns>比较结果数组</returns>
	Public Function Gt(Of T As INumeric)(A As Array(Of T), B As T) As Array(Of Boolean)
		Return A > B
	End Function
	''' <summary>
	''' 确定小于。对两个数组对应位置判断左边是否小于右边，返回<see cref="Boolean"/>结果存于新数组的对应位置。数组元素必须实现<see cref="INumeric"/>
	''' </summary>
	''' <typeparam name="T">比较数据类型</typeparam>
	''' <param name="A">左数组</param>
	''' <param name="B">右数组</param>
	''' <returns>比较结果数组</returns>
	Public Function Lt(Of T As INumeric)(A As Array(Of T), B As Array(Of T)) As Array(Of Boolean)
		Return A < B
	End Function
	''' <summary>
	''' 确定小于。每个元素判断是否小于常数，返回<see cref="Boolean"/>结果存于新数组的对应位置。数组元素必须实现<see cref="INumeric"/>
	''' </summary>
	''' <typeparam name="T"></typeparam>
	''' <param name="A"></param>
	''' <param name="B"></param>
	''' <returns></returns>
	Public Function Lt(Of T As INumeric)(A As Array(Of T), B As T) As Array(Of Boolean)
		Return A < B
	End Function
End Module