Public Module ElMat
	Public Delegate Function Funbsx(A, B)
	''' <summary>
	''' ���ﲻ�ᴴ�������飬��˿����õײ��<see cref="Array"/>
	''' </summary>
	Private Sub ����ݹ�(Of T1, T2)(ԭ����1 As Array(Of T1), ԭ����2 As Array(Of T2), ��ά���� As Integer(), ��ά�� As Byte, ������1 As Array, ������2 As Array, ��ǰ���� As Integer(), ��ǰά�� As Byte, ԭ����1 As Integer(), ԭ����2 As Integer())
		Dim b As Integer = ԭ����1.Size(��ǰά��), c As Integer = ԭ����2.Size(��ǰά��)
		If ��ǰά�� < ��ά�� - 1 Then
			For a As Integer = 0 To ��ά����(��ǰά��) - 1
				��ǰ����(��ǰά��) = a
				If ��ǰά�� < ԭ����1.NDims Then ԭ����1(��ǰά��) = a Mod b
				If ��ǰά�� < ԭ����2.NDims Then ԭ����2(��ǰά��) = a Mod c
				����ݹ�(ԭ����1, ԭ����2, ��ά����, ��ά��, ������1, ������2, ��ǰ����, ��ǰά�� + 1, ԭ����1, ԭ����2)
			Next
		Else
			For a As Integer = 0 To ��ά����(��ǰά��) - 1
				��ǰ����(��ǰά��) = a
				If ��ǰά�� < ԭ����1.NDims Then ԭ����1(��ǰά��) = a Mod b
				If ��ǰά�� < ԭ����2.NDims Then ԭ����2(��ǰά��) = a Mod c
				������1.SetValue(ԭ����1(ԭ����1), ��ǰ����)
				������2.SetValue(ԭ����2(ԭ����2), ��ǰ����)
			Next
		End If
	End Sub
	''' <summary>
	''' �ж���������ߴ��Ƿ�ƥ��
	''' </summary>
	Private Function ƥ��(Of T1, T2)(����1 As Array(Of T1), ����2 As Array(Of T2)) As Boolean
		Dim a As Byte = ����1.NDims, b As Byte = ����2.NDims
		If a = b Then
			Dim d As Integer() = ����1.Size, e As Integer() = ����2.Size
			For c As Byte = 0 To a - 1
				If d(c) <> e(c) Then
					Return False
				End If
			Next
			Return True
		Else
			Return False
		End If
	End Function
	''' <summary>
	''' ��������һ���ᴴ�������飬��ʹ�û��������<see cref="Array"/>Ҳһ�ɸĳ�<see cref="Array(Of T)"/>
	''' </summary>
	Friend Function ����(Of T1, T2)(ByRef ����1 As Array(Of T1), ByRef ����2 As Array(Of T2)) As Integer()
		If ƥ��(����1, ����2) Then Return ����1.Size
		Dim b As Byte = Math.Max(����1.NDims, ����2.NDims), c(b - 1) As Integer, h As Integer, i As Integer
		For a As Byte = 0 To b - 1
			h = ����1.Size(a)
			i = ����2.Size(a)
			If h = i Then
				c(a) = h
			ElseIf h < i Then
				c(a) = i
			Else
				c(a) = h
			End If
		Next
		Dim d As New Array(Of T1)(c), e As New Array(Of T2)(c)
		����ݹ�(����1, ����2, c, b, d, e, Zeros(Of Integer)(b), 0, Zeros(Of Integer)(����1.NDims), Zeros(Of Integer)(����2.NDims))
		����1 = d
		����2 = e
		Return c
	End Function
	''' <summary>
	''' ��������Ͳ�����ί�е�Ҫ��
	''' </summary>
	Friend Sub BsxFun�ݹ�(���� As Funbsx, ����1 As Array, ����2 As Array, ά���� As Byte, ��ά���� As Integer(), ������ As Array, ��ǰά�� As Byte, ��ǰ���� As Integer())
		If ��ǰά�� < ά���� - 1 Then
			For a As Integer = 0 To ��ά����(��ǰά��) - 1
				��ǰ����(��ǰά��) = a
				BsxFun�ݹ�(����, ����1, ����2, ά����, ��ά����, ������, ��ǰά�� + 1, ��ǰ����)
			Next
		Else
			For a As Integer = 0 To ��ά����(��ǰά��) - 1
				��ǰ����(��ǰά��) = a
				������.SetValue(����.Invoke(����1.GetValue(��ǰ����), ����2.GetValue(��ǰ����)), ��ǰ����)
			Next
		End If
	End Sub
	Private Sub Ones�ݹ�(Of T)(���� As Array, ����1 As T, ά���� As Byte, ��ά���� As Integer(), ��ǰά�� As Byte, ��ǰ���� As Integer())
		If ��ǰά�� < ά���� - 1 Then
			For a As Integer = 0 To ��ά����(��ǰά��) - 1
				��ǰ����(��ǰά��) = a
				Ones�ݹ�(����, ����1, ά����, ��ά����, ��ǰά�� + 1, ��ǰ����)
			Next
		Else
			For a As Integer = 0 To ��ά����(��ǰά��) - 1
				��ǰ����(��ǰά��) = a
				����.SetValue(����1, ��ǰ����)
			Next
		End If
	End Sub
	''' <summary>
	''' ���ﲻ���������飬���õײ�<see cref="Array"/>
	''' </summary>
	Private Sub Cat�ݹ�(Of T)(Դ���� As Array(Of T), Ŀ������ As Array, ��ǰά�� As Byte, Դ���� As Integer(), Ŀ������ As Integer(), Ŀ��ά�� As Byte, Ŀ��ά���ۻ��� As Integer)
		If ��ǰά�� < Ŀ������.Rank - 1 Then
			If ��ǰά�� = Ŀ��ά�� Then
				For a As Integer = 0 To Դ����.Size(��ǰά��) - 1
					Դ����(��ǰά��) = a
					Ŀ������(��ǰά��) = Ŀ��ά���ۻ��� + a
					Cat�ݹ�(Դ����, Ŀ������, ��ǰά�� + 1, Դ����, Ŀ������, Ŀ��ά��, Ŀ��ά���ۻ���)
				Next
			Else
				For a As Integer = 0 To Դ����.Size(��ǰά��) - 1
					Դ����(��ǰά��) = a
					Ŀ������(��ǰά��) = a
					Cat�ݹ�(Դ����, Ŀ������, ��ǰά�� + 1, Դ����, Ŀ������, Ŀ��ά��, Ŀ��ά���ۻ���)
				Next
			End If
		Else
			If ��ǰά�� = Ŀ��ά�� Then
				For a As Integer = 0 To Դ����.Size(��ǰά��) - 1
					Դ����(��ǰά��) = a
					Ŀ������(��ǰά��) = Ŀ��ά���ۻ��� + a
					Ŀ������.SetValue(Դ����(Դ����), Ŀ������)
				Next
			Else
				For a As Integer = 0 To Դ����.Size(��ǰά��) - 1
					Դ����(��ǰά��) = a
					Ŀ������(��ǰά��) = a
					Ŀ������.SetValue(Դ����(Դ����), Ŀ������)
				Next
			End If
		End If
	End Sub
#Region "Public"
	''' <summary>
	''' ����ȫ�����顣��ͬ��MATLAB�����ֻ��һ����С�����������ص�1ά��������������������
	''' </summary>
	''' <typeparam name="T">Ҫ�������������ͣ��ࣩ</typeparam>
	''' <param name="sz">ÿ��ά�ȵĴ�С</param>
	''' <returns>ȫ������</returns>
	Public Function Zeros(Of T)(ParamArray sz As Integer()) As Array(Of T)
		Return New Array(Of T)(sz)
	End Function
	''' <summary>
	''' ����ȫ��<see cref="Double"/>���顣��ͬ��MATLAB�����ֻ��һ����С�����������ص�1ά��������������������
	''' </summary>
	''' <param name="sz">ÿ��ά�ȵĴ�С</param>
	''' <returns>ȫ��<see cref="Double"/>����</returns>
	Public Function Zeros(ParamArray sz As Integer()) As Array(Of Double)
		Return Zeros(Of Double)(sz)
	End Function
	''' <summary>
	''' ����ȫ��Ϊ 1 �����顣��ͬ��MATLAB�����ֻ��һ����С�����������ص�1ά��������������������
	''' </summary>
	''' <typeparam name="T">�����</typeparam>
	''' <param name="sz">ÿ��ά�ȵĴ�С</param>
	''' <returns>ȫ1����</returns>
	Public Function Ones(Of T)(ParamArray sz As Integer()) As Array(Of T)
		Dim a As New Array(Of T)(sz)
		Ones�ݹ�(a, CType(DirectCast(1, Object), T), sz.Length, sz, 0, Zeros(Of Integer)(sz.Length))
		Return a
	End Function
	''' <summary>
	''' ����ȫ��Ϊ 1 ��<see cref="Double"/>���顣��ͬ��MATLAB�����ֻ��һ����С�����������ص�1ά��������������������
	''' </summary>
	''' <param name="sz">ÿ��ά�ȵĴ�С</param>
	''' <returns>ȫ1<see cref="Double"/>����</returns>
	Public Function Ones(ParamArray sz As Integer()) As Array(Of Double)
		Return Ones(Of Double)(sz)
	End Function
	''' <summary>
	''' ����������Ӧ�ð�Ԫ�����㣨������ʽ��չ������ͬ��MATLAB���������ʽ��չ���ӽ�׳��������ѭ����䷽ʽ��ʹ������<c>Ones(2,2)+Ones(4,4)=Ones(4,4)+Ones(4,4)</c>
	''' </summary>
	''' <typeparam name="T">���Ԫ������</typeparam>
	''' <param name="fun">ҪӦ�õĶ�Ԫ����</param>
	''' <param name="A">��������</param>
	''' <param name="B">��������</param>
	''' <returns>��������</returns>
	Public Function BsxFun(Of TIn1, TIn2, TOut)(fun As Funbsx, A As Array(Of TIn1), B As Array(Of TIn2)) As Array(Of TOut)
		Dim c As Integer() = ����(A, B)
		Dim d As New Array(Of TOut)(c)
		BsxFun�ݹ�(fun, A, B, c.Length, c, d, 0, Zeros(Of Integer)(c.Length))
		Return d
	End Function
	''' <summary>
	''' �ع�����
	''' </summary>
	''' <typeparam name="T">Ԫ������</typeparam>
	''' <param name="A">�������飬�������ȷ���ĳ���</param>
	''' <param name="sz">��ά���ȣ�����ʹ��һ��Nothing��ʾ�Զ��ƶϸ�ά����</param>
	''' <returns>�ع�������</returns>
	Public Function Reshape(Of T)(A As Array(Of T), ParamArray sz As UInteger?()) As Array(Of T)
		Return A.Reshape(sz)
	End Function
	''' <summary>
	''' �û�����ά�ȣ�ά��˳���0��ʼ��
	''' </summary>
	''' <typeparam name="T">Ԫ�����ͣ�ȱʡ<see cref="Object"/></typeparam>
	''' <param name="A">��������</param>
	''' <param name="dimorder">ά��˳�򡣲�ͬ��MATLAB����0��ʼ</param>
	''' <returns>�û�ά�ȵ�����</returns>
	Public Function Permute(Of T)(A As Array(Of T), ParamArray dimorder As Byte()) As Array(Of T)
		Return A.Permute(dimorder)
	End Function
#Region "Size"
	''' <summary>
	''' �����С��ĩβ�ĵ�һά�Ȼ���ԡ�
	''' </summary>
	''' <typeparam name="T">��������</typeparam>
	''' <param name="A">��������</param>
	''' <returns>�����ά�ߴ繹�ɵ�����</returns>
	Public Function Size(Of T)(A As Array(Of T)) As Integer()
		Return A.Size
	End Function
	''' <summary>
	''' �����С
	''' </summary>
	''' <typeparam name="T">��������</typeparam>
	''' <param name="A">��������</param>
	''' <param name="[dim]">��ѯ��ά��</param>
	''' <returns>�����С</returns>
	Public Function Size(Of T)(A As Array(Of T), [dim] As Byte) As Integer
		Return A.Size([dim])
	End Function
#End Region
	''' <summary>
	''' �����ά����Ŀ�����Խϸߵĵ�һά�ȡ�
	''' </summary>
	''' <typeparam name="T">��������</typeparam>
	''' <param name="A">��������</param>
	''' <returns>ά����</returns>
	Public Function NDims(Of T)(A As Array(Of T)) As Byte
		Return A.NDims
	End Function
	''' <summary>
	''' �������顣���������ڴ���ά�������ά�ȱ���ƥ�䣬�˺����������Ƿ�ƥ�䣬��ƥ������鴮�����ܻᱨ������δ֪�����
	''' </summary>
	''' <typeparam name="T">Ԫ�����ͣ�ȱʡ<see cref="Object"/></typeparam>
	''' <param name="[dim]">���������ά��</param>
	''' <param name="A">�����б�</param>
	''' <returns>����������</returns>
	Public Function Cat(Of T)([dim] As Byte, ParamArray A As Array(Of T)()) As Array(Of T)
		Dim b As Array(Of Integer) = A(0).Size, c As Integer, d As Array(Of T)
		For Each d In A
			c += d.Size([dim])
		Next
		b([dim]) = c
		c = b.Numel - 1
		For e As Byte = 0 To c
			If b(e) = 0 Then b(e) = 1
		Next
		Dim g As New Array(Of T)(CType(b, Integer()))
		For e As Byte = 0 To c
			b(e) = 0
		Next
		b([dim]) = -1
		Dim f(c) As Integer
		For Each d In A
			Cat�ݹ�(d, g, 0, f, b, [dim], b([dim]) + 1)
		Next
		Return g
	End Function
	''' <summary>
	''' �������� A �е�Ԫ����Ŀ n ��ͬ�� prod(size(A))��
	''' </summary>
	''' <typeparam name="T">��������</typeparam>
	''' <param name="A">��������</param>
	''' <returns>Ԫ����Ŀ</returns>
	Function Numel(Of T)(A As Array(Of T)) As Integer
		Return A.Numel
	End Function
#End Region
End Module