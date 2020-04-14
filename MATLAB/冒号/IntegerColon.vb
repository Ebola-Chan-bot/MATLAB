Public Structure IntegerColon
	ReadOnly 开始 As Integer, 公差 As Integer, 结束 As Integer?
	Friend Sub New(开始 As Integer, 公差 As Integer, 结束 As Integer?)
		Me.开始 = 开始
		Me.公差 = 公差
		Me.结束 = 结束
	End Sub
	Friend Function ToIndex(结束 As Integer) As Integer()
		Dim a As Integer = If(Me.结束, 结束), e As Integer = Math.Floor((a - 开始) / 公差), b(e) As Integer, c As Integer = 开始
		For d As Integer = 0 To e
			b(d) = c
			c += 公差
		Next
		Return b
	End Function
End Structure
