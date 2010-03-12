Option Strict On
Option Explicit On

Imports System
Imports System.Diagnostics

Public Class TraceEntry
	Public RunGuid As System.Guid
	Public TraceLevel As TraceLevel
	Public ValueName As String
	Public Value As Object
	Public TraceIssue As TraceIssues
	Public Issue As String
	Public NameSpaceName As String
	Public ClassName As String
	Public MethodName As String
	Public MethodBase As Reflection.MethodBase
	Public Row As Int32
	Public Col As Int32

	Public Sub New( _
							ByVal runGUID As System.Guid, _
							ByVal stackFrame As StackFrame, _
							ByVal params() As Object, _
							ByVal traceLevel As TraceLevel, _
							ByVal value As Object, _
							ByVal valueName As String, _
							ByVal traceIssue As TraceIssues, _
							ByVal issue As String)
		SetStack(stackFrame, params)
		Me.RunGuid = runGUID
		Me.TraceLevel = traceLevel
		Me.Value = value
		Me.ValueName = valueName
		Me.TraceIssue = traceIssue
		Me.Issue = issue
	End Sub

	Public Overrides Function ToString() As String
		Return FormatTraceOutput(Me.RunGuid, Me.NameSpaceName, Me.ClassName, _
								Me.MethodName, Me.Row, Me.Col, Me.ValueName, _
								Me.ValueName, Me.TraceLevel, Me.TraceIssue, Me.Issue)
	End Function

	Public Shared Function FormatTraceOutput( _
							ByVal runGUID As System.Guid, _
							ByVal stackFrame As StackFrame, _
							ByVal params() As Object, _
							ByVal traceLevel As TraceLevel, _
							ByVal value As Object, _
							ByVal valueName As String, _
							ByVal traceIssue As TraceIssues, _
							ByVal issue As String) _
							As String
		Dim nspace As String = String.Empty
		Dim className As String = String.Empty
		Dim methodName As String = String.Empty
		Dim row As Int32
		Dim col As Int32

		SetStack(stackFrame, params, nspace, className, methodName, row, col)
		Return FormatTraceOutput(runGUID, nspace, className, methodName, row, col, _
								value, valueName, traceLevel, traceIssue, issue)

	End Function

	Public Shared Function FormatTraceOutput( _
							ByVal runGUID As System.Guid, _
							ByVal nspace As String, _
							ByVal className As String, _
							ByVal methodName As String, _
							ByVal row As Int32, _
							ByVal col As Int32, _
							ByVal value As Object, _
							ByVal valueName As String, _
							ByVal traceLevel As TraceLevel, _
							ByVal traceIssue As TraceIssues, _
							ByVal issue As String) _
							As String
		Dim sVal As String
		If value Is Nothing Then
			sVal = ""
		Else
			sVal = value.ToString
		End If
		If sVal.Length > 0 Then
			If valueName Is Nothing OrElse valueName = "" Then
				sVal = "(Value=" & sVal & ")"
			Else
				sVal = "(" & valueName & "=" & sVal & ")"
			End If
		End If
		'Return runGUID.ToString & "-" & DateTime.Now.ToString & "-" & _
		'         nspace & "." & className & "." & methodName & _
		'         " row,col=" & row.ToString & "," & col.ToString & _
		'         sVal & " level=" & traceLevel.ToString() & " - " & issue
		Return "(" & traceLevel.ToString() & ") " & issue.Trim() & "; " & nspace & "." & className & "." & methodName & " (" & row.ToString & _
			"," & col.ToString & ") " & sVal
	End Function


#Region "Private Support Routines"

	Private Sub SetStack( _
					 ByVal stackFrame As StackFrame, _
					 ByVal params() As Object)

		' string[] stackdetails = stackFrame.GetMethod().Trim().Split('.')
		MethodBase = stackFrame.GetMethod()
		NameSpaceName = MethodBase.ReflectedType.Namespace
		ClassName = MethodBase.ReflectedType.Name
		MethodName = MethodWithParams(MethodBase, params)
		Row = stackFrame.GetFileLineNumber
		Col = stackFrame.GetFileColumnNumber
	End Sub

	Private Shared Sub SetStack( _
					 ByVal stackFrame As StackFrame, _
					 ByVal params() As Object, _
					 ByRef nspace As String, _
					 ByRef className As String, _
					 ByRef methodName As String, _
					 ByRef row As Int32, _
					 ByRef col As Int32)

		' string[] stackdetails = stackFrame.GetMethod().Trim().Split('.')
		Dim method As Reflection.MethodBase = stackFrame.GetMethod()
		nspace = method.ReflectedType.Namespace
		className = method.ReflectedType.Name
		methodName = MethodWithParams(method, params)
		row = stackFrame.GetFileLineNumber
		col = stackFrame.GetFileColumnNumber

	End Sub

	Private Shared Function MethodWithParams( _
					 ByVal method As Reflection.MethodBase, _
					 ByVal params() As Object) _
					 As String
		' The decision not to use a StringBuilder was deliberate
		' I don't expect more than six parameters often, and I do 
		' expect frequently to have none. 
		Dim ret As String = method.Name & "("
		Dim methodparams As Reflection.ParameterInfo()
		methodparams = method.GetParameters()
		If Tools.SkipArguments Or methodparams.Length = 0 Then
			Return ret & ")"
		Else
			If params Is Nothing Then
				For i As Int32 = 0 To methodparams.GetUpperBound(0)
					ret &= methodparams(i).ParameterType.Name & ", "
				Next
				Return ret.Substring(0, ret.Length - 1) & ")"
			ElseIf methodparams.Length <> params.Length Then
				Return ret & " trace param info is inconsistent )"
			Else
				For i As Int32 = 0 To methodparams.GetUpperBound(0)
					If methodparams(i).ParameterType Is GetType(System.String) Then
						ret &= methodparams(i).ParameterType.Name & " " & _
										 methodparams(i).Name & "=" & _
										 """" & params(i).ToString & """, "
						'ElseIf GetType(Windows.Forms.Control).IsAssignableFrom(methodparams(i).ParameterType) Then
						'   ret &= methodparams(i).ParameterType.Name & " " & _
						'            methodparams(i).Name & "=" & _
						'            """" & CType(params(i), Windows.Forms.Control).Name & """, "
					Else
						ret &= methodparams(i).ParameterType.Name & " " & _
										 methodparams(i).Name & "=" & _
										 params(i).ToString & ", "
					End If
				Next
				Return ret.Substring(0, ret.Length - 2) & ")"
			End If
		End If
	End Function
#End Region

End Class
