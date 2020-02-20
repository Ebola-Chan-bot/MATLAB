''' <summary>
''' 冒号表达式，由<see cref="Colon"/>函数得到，支持MATLAB数组索引的<see cref="[End]"/>功能
''' </summary>
Public Structure ColonExpression
	Implements IToVector, IEnumerable(Of INumeric)
	ReadOnly 公差 As INumeric
	Private 开始 As INumeric, 结束 As INumeric
	Shared ReadOnly 默认公差 As New MUInt32(1)
	Sub New(开始 As INumeric, 结束 As INumeric)
		Me.开始 = 开始
		公差 = New MUInt32(1)
		Me.结束 = 结束
	End Sub
	Sub New(开始 As INumeric, 公差 As INumeric, 结束 As INumeric)
		Me.开始 = 开始
		Me.公差 = 公差
		Me.结束 = 结束
	End Sub
	Sub New(开始 As Double, 结束 As Double)
		Me.开始 = New MDouble(开始)
		公差 = New MUInt32(1)
		Me.结束 = New MDouble(结束)
	End Sub
	Sub New(开始 As Double, 公差 As Double, 结束 As Double)
		Me.开始 = New MDouble(开始)
		Me.公差 = New MDouble(公差)
		Me.结束 = New MDouble(结束)
	End Sub
	Public Function ToIndex([End] As UInteger) As UInteger()
		If 开始.Equals(Ops.End) Then 开始 = New MInt32([End])
		If 结束.Equals(Ops.End) Then 结束 = New MInt32([End])
		Return ToUInteger()
	End Function
	Public Function ToMDecimal() As MDecimal() Implements IToVector.ToMDecimal
		Return AsParallel.AsOrdered.[Select](Function(a As INumeric) New MDecimal(a)).ToArray
	End Function

	Public Function ToMDouble() As MDouble() Implements IToVector.ToMDouble
		Return AsParallel.AsOrdered.[Select](Function(a As INumeric) New MDouble(a)).ToArray
	End Function

	Public Function ToMSingle() As MSingle() Implements IToVector.ToMSingle
		Return AsParallel.AsOrdered.[Select](Function(a As INumeric) New MSingle(a)).ToArray
	End Function

	Public Function ToMInt8() As MInt8() Implements IToVector.ToMInt8
		Return AsParallel.AsOrdered.[Select](Function(a As INumeric) New MInt8(a)).ToArray
	End Function

	Public Function ToMInt16() As MInt16() Implements IToVector.ToMInt16
		Return AsParallel.AsOrdered.[Select](Function(a As INumeric) New MInt16(a)).ToArray
	End Function

	Public Function ToMInt32() As MInt32() Implements IToVector.ToMInt32
		Return AsParallel.AsOrdered.[Select](Function(a As INumeric) New MInt32(a)).ToArray
	End Function

	Public Function ToMInt64() As MInt64() Implements IToVector.ToMInt64
		Return AsParallel.AsOrdered.[Select](Function(a As INumeric) New MInt64(a)).ToArray
	End Function

	Public Function ToMUInt8() As MUInt8() Implements IToVector.ToMUInt8
		Return AsParallel.AsOrdered.[Select](Function(a As INumeric) New MUInt8(a)).ToArray
	End Function

	Public Function ToMUInt16() As MUInt16() Implements IToVector.ToMUInt16
		Return AsParallel.AsOrdered.[Select](Function(a As INumeric) New MUInt16(a)).ToArray
	End Function

	Public Function ToMUInt32() As MUInt32() Implements IToVector.ToMUInt32
		Return AsParallel.AsOrdered.[Select](Function(a As INumeric) New MUInt32(a)).ToArray
	End Function

	Public Function ToMUInt64() As MUInt64() Implements IToVector.ToMUInt64
		Return AsParallel.AsOrdered.[Select](Function(a As INumeric) New MUInt64(a)).ToArray
	End Function

	Public Function ToDecimal() As Decimal() Implements IToVector.ToDecimal
		Return AsParallel.AsOrdered.[Select](Function(a As INumeric) CDec(a.RawData)).ToArray
	End Function

	Public Function ToDouble() As Double() Implements IToVector.ToDouble
		Return AsParallel.AsOrdered.[Select](Function(a As INumeric) CDbl(a.RawData)).ToArray
	End Function

	Public Function ToSingle() As Single() Implements IToVector.ToSingle
		Return AsParallel.AsOrdered.[Select](Function(a As INumeric) CSng(a.RawData)).ToArray
	End Function

	Public Function ToSByte() As SByte() Implements IToVector.ToSByte
		Return AsParallel.AsOrdered.[Select](Function(a As INumeric) CSByte(a.RawData)).ToArray
	End Function

	Public Function ToShort() As Short() Implements IToVector.ToShort
		Return AsParallel.AsOrdered.[Select](Function(a As INumeric) CShort(a.RawData)).ToArray
	End Function

	Public Function ToInteger() As Integer() Implements IToVector.ToInteger
		Return AsParallel.AsOrdered.[Select](Function(a As INumeric) CInt(a.RawData)).ToArray
	End Function

	Public Function ToLong() As Long() Implements IToVector.ToLong
		Return AsParallel.AsOrdered.[Select](Function(a As INumeric) CLng(a.RawData)).ToArray
	End Function

	Public Function ToByte() As Byte() Implements IToVector.ToByte
		Return AsParallel.AsOrdered.[Select](Function(a As INumeric) CByte(a.RawData)).ToArray
	End Function

	Public Function ToUShort() As UShort() Implements IToVector.ToUShort
		Return AsParallel.AsOrdered.[Select](Function(a As INumeric) CUShort(a.RawData)).ToArray
	End Function

	Public Function ToUInteger() As UInteger() Implements IToVector.ToUInteger
		Return AsParallel.AsOrdered.[Select](Function(a As INumeric) CUInt(a.RawData)).ToArray
	End Function

	Public Function ToULong() As ULong() Implements IToVector.ToULong
		Return AsParallel.AsOrdered.[Select](Function(a As INumeric) CULng(a.RawData)).ToArray
	End Function
	Private Class ColonEnumerator
		Implements IEnumerator(Of INumeric)
		ReadOnly 开始 As INumeric, 公差 As INumeric, 结束 As INumeric
		Private iCurrent As INumeric
		Sub New(开始 As INumeric, 公差 As INumeric, 结束 As INumeric)
			Me.开始 = 开始
			Me.公差 = 公差
			Me.结束 = 结束
		End Sub
		Public ReadOnly Property Current As INumeric Implements IEnumerator(Of INumeric).Current
			Get
				Return iCurrent
			End Get
		End Property

		Private ReadOnly Property IEnumerator_Current As Object Implements IEnumerator.Current
			Get
				Return iCurrent
			End Get
		End Property

		Public Sub Reset() Implements IEnumerator.Reset
			iCurrent = 开始
		End Sub

		Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
			Static 常数0 As New MInt32(0)
			If iCurrent Is Nothing AndAlso (结束.Gt(开始) = 公差.Gt(常数0) OrElse 结束.Equals(开始)) Then
				iCurrent = 开始
				Return True
			Else
				Dim a As INumeric = iCurrent.Plus(公差)
				If a.Lt(结束) = 公差.Gt(常数0) OrElse a.Equals(结束) Then
					iCurrent = a
					Return True
				Else
					Return False
				End If
			End If
		End Function

#Region "IDisposable Support"
		Private disposedValue As Boolean ' 要检测冗余调用

		' IDisposable
		Protected Overridable Sub Dispose(disposing As Boolean)
			If Not disposedValue Then
				If disposing Then
					' TODO: 释放托管状态(托管对象)。
				End If

				' TODO: 释放未托管资源(未托管对象)并在以下内容中替代 Finalize()。
				' TODO: 将大型字段设置为 null。
			End If
			disposedValue = True
		End Sub

		' TODO: 仅当以上 Dispose(disposing As Boolean)拥有用于释放未托管资源的代码时才替代 Finalize()。
		'Protected Overrides Sub Finalize()
		'    ' 请勿更改此代码。将清理代码放入以上 Dispose(disposing As Boolean)中。
		'    Dispose(False)
		'    MyBase.Finalize()
		'End Sub

		' Visual Basic 添加此代码以正确实现可释放模式。
		Public Sub Dispose() Implements IDisposable.Dispose
			' 请勿更改此代码。将清理代码放入以上 Dispose(disposing As Boolean)中。
			Dispose(True)
			' TODO: 如果在以上内容中替代了 Finalize()，则取消注释以下行。
			' GC.SuppressFinalize(Me)
		End Sub
#End Region
	End Class
	Public Function GetEnumerator() As IEnumerator(Of INumeric) Implements IEnumerable(Of INumeric).GetEnumerator
		Return New ColonEnumerator(开始, 公差, 结束)
	End Function

	Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
		Return New ColonEnumerator(开始, 公差, 结束)
	End Function
	Public Overrides Function ToString() As String
		Dim a As New Text.StringBuilder
		If 开始.Equals([End]) Then
			a.Append("end:")
		Else
			a.Append(开始).Append(":")
		End If
		If Not 公差.Equals(默认公差) Then a.Append(公差).Append(":")
		If 结束.Equals([End]) Then
			a.Append("end")
		Else
			a.Append(结束)
		End If
		Return a.ToString
	End Function
End Structure
