Public Module ElFun
	''' <summary>
	''' 以弧度为单位返回 X 各元素的反正切 (tan-1)。
	''' </summary>
	''' <param name="X">角正切，指定为标量、向量、矩阵或多维数组。如果 X 为非标量，则按元素执行 atan 运算。</param>
	Public Function Atan(X As TypedArray(Of Single)) As SingleArray
		Return New SingleArray(X.Size.ToArray, (From b As Single In X.本体 Select CSng(Math.Atan(b))).ToArray)
	End Function
End Module
