Public Module ElFun
	Function Atan(A As TypedArray(Of Single)) As SingleArray
		Return New SingleArray(A.Size.ToArray, (From b As Single In A.本体 Select CSng(Math.Atan(b))).ToArray)
	End Function
End Module
