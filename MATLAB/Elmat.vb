Public Module ElMat
	''' <summary>
	''' ���ﲻ���������飬���õײ�<see cref="Array"/>
	''' </summary>
	Private Sub Cat�ݹ�(Of T)(Դ���� As Array(Of T), Ŀ������ As Array(Of T), ��ǰά�� As Byte, Դ���� As UInteger(), Ŀ������ As UInteger(), Ŀ��ά�� As Byte, Ŀ��ά���ۻ��� As UInteger)
		If ��ǰά�� < Ŀ������.NDims - 1 Then
			If ��ǰά�� = Ŀ��ά�� Then
				For a As UInteger = 0 To Դ����.Size(��ǰά��) - 1
					Դ����(��ǰά��) = a
					Ŀ������(��ǰά��) = Ŀ��ά���ۻ��� + a
					Cat�ݹ�(Դ����, Ŀ������, ��ǰά�� + 1, Դ����, Ŀ������, Ŀ��ά��, Ŀ��ά���ۻ���)
				Next
			Else
				For a As UInteger = 0 To Դ����.Size(��ǰά��) - 1
					Դ����(��ǰά��) = a
					Ŀ������(��ǰά��) = a
					Cat�ݹ�(Դ����, Ŀ������, ��ǰά�� + 1, Դ����, Ŀ������, Ŀ��ά��, Ŀ��ά���ۻ���)
				Next
			End If
		Else
			If ��ǰά�� = Ŀ��ά�� Then
				If ��ǰά�� < Դ����.Length Then
					For a As UInteger = 0 To Դ����.Size(��ǰά��) - 1
						Դ����(��ǰά��) = a
						Ŀ������(��ǰά��) = Ŀ��ά���ۻ��� + a
						Ŀ������.SetValue(Դ����.GetValue(Դ����), Ŀ������)
					Next
				Else
					For a As UInteger = 0 To Դ����.Size(��ǰά��) - 1
						Ŀ������(��ǰά��) = Ŀ��ά���ۻ��� + a
						Ŀ������.SetValue(Դ����.GetValue(Դ����), Ŀ������)
					Next
				End If
			Else
				For a As UInteger = 0 To Դ����.Size(��ǰά��) - 1
					Դ����(��ǰά��) = a
					Ŀ������(��ǰά��) = a
					Ŀ������.SetValue(Դ����.GetValue(Դ����), Ŀ������)
				Next
			End If
		End If
	End Sub
	''' <summary>
	''' ����������ɲ�����������Ϊ<c>typename</c>�� sz1��...��szN ���飬���� sz1,...,szN ָʾÿ��ά�ȵĴ�С�����磬<c>Zeros(2, 3)</c>������һ�� 2��3 ����
	''' </summary>
	''' <typeparam name="typename">Ҫ�������������ͣ��ࣩ</typeparam>
	''' <param name="sz">ÿ��ά�ȵĴ�С</param>
	''' <returns>ȫ������</returns>
	Public Function Zeros(Of typename As INumeric)(ParamArray sz As UInteger()) As Array(Of typename)
		Return New Array(Of typename)(sz)
	End Function
	''' <summary>
	''' ����������ɲ�����������Ϊ<see cref="MDouble"/>�� sz1��...��szN ���飬���� sz1,...,szN ָʾÿ��ά�ȵĴ�С�����磬<c>Zeros(2, 3)</c>������һ�� 2��3 ����
	''' </summary>
	''' <param name="sz">ÿ��ά�ȵĴ�С</param>
	''' <returns>ȫ��<see cref="MDouble"/>����</returns>
	Public Function Zeros(ParamArray sz As UInteger()) As Array(Of MDouble)
		Return Zeros(Of MDouble)(sz)
	End Function
	''' <summary>
	''' ����һ���� 1 ��ɲ�����������Ϊ classname �� sz1��...��szN ���顣
	''' </summary>
	''' <typeparam name="classname">�����</typeparam>
	''' <param name="sz">ÿ��ά�ȵĴ�С</param>
	''' <returns>ȫ1����</returns>
	Public Function Ones(Of classname As {INumeric, New})(ParamArray sz As UInteger()) As Array(Of classname)
		Ones = New Array(Of classname)(sz)
		Dim a As New classname
		a.SetValue(1)
		Ones.���� = Enumerable.Repeat(a, Ones.Numel).ToArray
	End Function
	''' <summary>
	''' ������ 1 ��ɵ� sz1��...��szN <see cref="MDouble"/>���飬���� sz1,...,szN ָʾÿ��ά�ȵĴ�С�����磬<c>Ones(2, 3)</c>������ 1 ��ɵ� 2��3 ���顣
	''' </summary>
	''' <param name="sz">ÿ��ά�ȵĴ�С</param>
	''' <returns>ȫ1<see cref="MDouble"/>����</returns>
	Public Function Ones(ParamArray sz As UInteger()) As Array(Of MDouble)
		Return Ones(Of MDouble)(sz)
	End Function
	''' <summary>
	''' ���ﲻ�ᴴ�������飬��˿����õײ��<see cref="Array"/>
	''' </summary>
	Private Sub ����ݹ�(Of T1, T2)(ԭ����1 As Array(Of T1), ԭ����2 As Array(Of T2), ��ά���� As UInteger(), ��ά�� As Byte, ������1 As Array(Of T1), ������2 As Array(Of T2), ��ǰ���� As UInteger(), ��ǰά�� As Byte, ԭ����1 As UInteger(), ԭ����2 As UInteger())
		Dim b As UInteger = ԭ����1.Size(��ǰά��), c As UInteger = ԭ����2.Size(��ǰά��)
		If ��ǰά�� < ��ά�� - 1 Then
			For a As UInteger = 0 To ��ά����(��ǰά��) - 1
				��ǰ����(��ǰά��) = a
				If ��ǰά�� < ԭ����1.NDims Then ԭ����1(��ǰά��) = a Mod b
				If ��ǰά�� < ԭ����2.NDims Then ԭ����2(��ǰά��) = a Mod c
				����ݹ�(ԭ����1, ԭ����2, ��ά����, ��ά��, ������1, ������2, ��ǰ����, ��ǰά�� + 1, ԭ����1, ԭ����2)
			Next
		Else
			For a As UInteger = 0 To ��ά����(��ǰά��) - 1
				��ǰ����(��ǰά��) = a
				If ��ǰά�� < ԭ����1.NDims Then ԭ����1(��ǰά��) = a Mod b
				If ��ǰά�� < ԭ����2.NDims Then ԭ����2(��ǰά��) = a Mod c
				������1.SetValue(ԭ����1.GetValue(ԭ����1), ��ǰ����)
				������2.SetValue(ԭ����2.GetValue(ԭ����2), ��ǰ����)
			Next
		End If
	End Sub
	''' <summary>
	''' ����������Ӧ�ð�Ԫ�����㣨������ʽ��չ������ͬ��MATLAB���������ʽ��չ���ӽ�׳��������ѭ����䷽ʽ��ʹ������<c>Ones(2, 2) + Ones(4, 4) = Ones(4, 4) + Ones(4, 4)</c>
	''' </summary>
	Public Function BsxFun(Of TIn1, TIn2, TOut)(fun As Func(Of TIn1, TIn2, TOut), A As Array(Of TIn1), B As Array(Of TIn2)) As Array(Of TOut)
		If A.Numel = 1 Then
			Dim c As TIn1 = A.����(0)
			Return New Array(Of TOut)(B.����.Select(Function(d As TIn2) fun.Invoke(c, d)).ToArray, B.Size)
		ElseIf B.Numel = 1 Then
			Dim d As TIn2 = B.����(0)
			Return New Array(Of TOut)(A.����.Select(Function(c As TIn1) fun.Invoke(c, d)).ToArray, A.Size)
		Else
			Dim c As IArray() = ����(A, B)
			A = c(0)
			B = c(1)
			Return New Array(Of TOut)(A.����.AsParallel.AsOrdered.Zip(B.����.AsParallel.AsOrdered, fun).ToArray, A.Size.ToArray)
		End If
	End Function
	''' <summary>
	''' �� A �ع�Ϊһ�� sz1��...��szN ���飬���� sz1,...,szN ָʾÿ��ά�ȵĴ�С������ָ�� Nothing �ĵ���ά�ȴ�С���Ա��Զ�����ά�ȴ�С����ʹ B �е�Ԫ������ A �е�Ԫ������ƥ�䡣���磬��� A ��һ�� 10��10 ������<c>Reshape(A, 2, 2, Nothing)</c>�� A �� 100 ��Ԫ���ع�Ϊһ�� 2��2��25 ���顣
	''' </summary>
	''' <typeparam name="T">Ԫ������</typeparam>
	''' <param name="A">�������飬�������ȷ���ĳ���</param>
	''' <param name="sz">��ά���ȣ�����ʹ��һ��Nothing��ʾ�Զ��ƶϸ�ά����</param>
	''' <returns>�ع�������</returns>
	Public Function Reshape(Of T)(A As Array(Of T), ParamArray sz As UInteger?()) As Array(Of T)
		Reshape = A.Clone
		Reshape.Reshape(sz)
	End Function
	''' <summary>
	''' �������� dimorder ָ����˳���������������ά�ȡ����磬<c>Permute(A, 1, 0)</c> �������� A ���к���ά�ȡ�
	''' </summary>
	''' <typeparam name="T">Ԫ������</typeparam>
	''' <param name="A">��������</param>
	''' <param name="dimorder">ά��˳�򡣲�ͬ��MATLAB����0��ʼ</param>
	''' <returns>�û�ά�ȵ�����</returns>
	Public Function Permute(Of T)(A As Array(Of T), ParamArray dimorder As Byte()) As Array(Of T)
		Return A.Permute(dimorder)
	End Function
	''' <summary>
	''' ����һ����������Ԫ�ذ��� A ����Ӧά�ȵĳ��ȡ����磬��� A ��һ�� 3��4 ������<c>Size(A)</c>��������<c>{3, 4}</c>��sz �ĳ���Ϊ<c>NDims(A)</c>��
	''' </summary>
	''' <typeparam name="T">��������</typeparam>
	''' <param name="A">��������</param>
	''' <returns>�����ά�ߴ繹�ɵ�����</returns>
	Public Function Size(Of T)(A As Array(Of T)) As UInteger()
		Return A.Size
	End Function
	''' <summary>
	''' ����ά�� dim �ĳ��ȡ�
	''' </summary>
	''' <typeparam name="T">��������</typeparam>
	''' <param name="A">��������</param>
	''' <param name="[dim]">��ѯ��ά��</param>
	''' <returns>�����С</returns>
	Public Function Size(Of T)(A As Array(Of T), [dim] As Byte) As UInteger
		Return A.Size([dim])
	End Function
	<Runtime.CompilerServices.Extension> Public Function Size(A As Array) As UInteger()
		Dim c As Byte = A.Rank - 1, d(c) As UInteger
		For b As Byte = 0 To c
			d(b) = A.GetLength(b)
		Next
		Return d
	End Function
	''' <summary>
	''' �������� A ��ά�������������<c>Size(A, [dim]) = 1</c>����Ե�β����һά�ȡ�
	''' </summary>
	''' <typeparam name="T">��������</typeparam>
	''' <param name="A">��������</param>
	''' <returns>ά����</returns>
	Public Function NDims(Of T)(A As Array(Of T)) As Byte
		Return A.NDims
	End Function
	''' <summary>
	''' ��ά�� dim ���� A1��A2������An�����÷�Ӧ��֤���������ڴ���ά�������ά�ȵȳ����˺���������м�顣
	''' </summary>
	''' <typeparam name="T">Ԫ�����ͣ�ȱʡ<see cref="Object"/></typeparam>
	''' <param name="[dim]">���������ά��</param>
	''' <param name="A">�����б�</param>
	''' <returns>����������</returns>
	Public Function Cat(Of T)([dim] As Byte, ParamArray A As Array(Of T)()) As Array(Of T)
		Dim b As Array(Of UInteger) = A(0).Size.ToArray, c As UInteger
		For Each d As Array(Of T) In A
			c += d.Size([dim])
		Next
		'��һ�����ܻ���չ����
		b([dim]) = c
		Dim h As UInteger() = b
		c = h.Length - 1
		For e As Byte = 0 To c
			If h(e) = 0 Then h(e) = 1
		Next
		Dim g As New Array(Of T)(h), i(c) As UInteger
		Dim f As UInteger()
		For Each d As Array(Of T) In A
			ReDim f(d.NDims - 1)
			Cat�ݹ�(d, g, 0, f, i, [dim], i([dim]))
			i([dim]) += 1
		Next
		Return g
	End Function
	''' <summary>
	''' �������� A �е�Ԫ����Ŀ
	''' </summary>
	''' <typeparam name="T">��������</typeparam>
	''' <param name="A">��������</param>
	''' <returns>Ԫ����Ŀ</returns>
	Public Function Numel(Of T)(A As Array(Of T)) As UInteger
		Return A.Numel
	End Function
End Module