Public Class SingleMean累积器
	Implements I累积器(Of Single, Single)
	Private 总和 As Single, 计数 As Single
	Public Sub 累积(积入值 As Single) Implements I累积器(Of Single, Single).累积
		总和 += 积入值
		计数 += 1
	End Sub

	Public Function 结果() As Single Implements I累积器(Of Single, Single).结果
		结果 = 总和 / 计数
		总和 = 0
		计数 = 0
	End Function
End Class
