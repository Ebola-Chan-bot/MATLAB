Public Class SingleSum累积器
	Implements I累积器(Of Single, Single)
	Private 总和 As Single
	Public Sub 累积(积入值 As Single) Implements I累积器(Of Single, Single).累积
		总和 += 积入值
	End Sub

	Public Function 结果() As Single Implements I累积器(Of Single, Single).结果
		结果 = 总和
		总和 = 0
	End Function
End Class
