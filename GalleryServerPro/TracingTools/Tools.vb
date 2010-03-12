Option Strict On
Option Explicit On 

Imports System
Imports System.Diagnostics


Public Enum TraceIssues
  ObjectIsNotNull
  IntLessThan
  IntGreaterThan
  IntInRange_High
  IntInRange_Low
  StringRequired
  StartingMethod
  InCondition
  InFor
  MarkSpot
End Enum

Public Class Tools

#Region "Class Level Fields"

  Private Const crlf As String = Microsoft.VisualBasic.ControlChars.CrLf
  Private Const tab As String = Microsoft.VisualBasic.ControlChars.Tab

  Private Shared mbKnownListeners As Boolean
  Private Shared mRunGUID As System.Guid

  Private Shared TSLevel As TraceSwitch = _
      New TraceSwitch("TraceLevelSwitch", "Trace Level Test Switch")
  Private Shared TSSkipEventArgs As BooleanSwitch = _
      New BooleanSwitch("SkipEventArgs", "Skip Event Arg details")
  Private Shared TSSkipArguments As BooleanSwitch = _
      New BooleanSwitch("SkipArguments", "Skip Argument details")

#End Region

#Region "Management Functions"
  Shared Sub New()
    mRunGUID = System.Guid.NewGuid
  End Sub

  ''' <summary>
  ''' Gets / sets a value indicating whether to not record any information about event arguments. If false, 
  ''' Reflection is used to query the EventArgs variable for those methods that pass it as a parameter.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Shared Property SkipEventArgs() As Boolean
    Get
      Return TSSkipEventArgs.Enabled
    End Get
    Set(ByVal Value As Boolean)
      TSSkipEventArgs.Enabled = Value
    End Set
  End Property

  ''' <summary>
  ''' Gets / sets a value indicating whether to not record any information about method arguments. If false,
  ''' Reflection is used to query the calling method for certain trace issues.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  Public Shared Property SkipArguments() As Boolean
    Get
      Return TSSkipArguments.Enabled
    End Get
    Set(ByVal Value As Boolean)
      TSSkipArguments.Enabled = Value
    End Set
  End Property

  ''' <summary>
  ''' Gets / sets the TraceLevel to use.
  ''' </summary>
  Public Shared Property TraceLevel() As TraceLevel
    Get
      Return TSLevel.Level
    End Get
    Set(ByVal Value As TraceLevel)
      TSLevel.Level = Value
    End Set
  End Property

  ''' <summary>
  ''' Remove all attached listeners.
  ''' </summary>
  ''' <remarks></remarks>
  Public Shared Sub ClearListeners()
    Trace.Listeners.Clear()
  End Sub

  ''' <summary>
  ''' Add the specified listener.
  ''' </summary>
  ''' <param name="traceListener">A TraceListener to add.</param>
  Public Shared Sub AddListener(ByVal traceListener As TraceListener)
    mbKnownListeners = False
    Trace.Listeners.Add(traceListener)
  End Sub

  ''' <summary>
  ''' Removes the specified listener.
  ''' </summary>
  ''' <param name="tracelistener">A TraceListener to remove.</param>
  Public Shared Sub RemoveListener(ByVal tracelistener As TraceListener)
    ' ENHANCE: If you're removing listeners created at the command prompt
    '          you'll need to add a name based remove process
    mbKnownListeners = False
    Trace.Listeners.Remove(tracelistener)
  End Sub

  ''' <summary>
  ''' Returns a value indicating whether there are currently any attached listeners.
  ''' </summary>
  ''' <returns>Returns a value indicating whether there are currently any attached listeners.</returns>
  Private Shared Function HasListeners() As Boolean
    Return Trace.Listeners.Count > 0
  End Function

#End Region


#Region "Sane parameter checking supporting Trace Levels (Trace Switches)"
  'NOTE: You can not refactor these public methods to call each other, or you will
  '      lose track of the stackframe

#Region "Object Can't be Nothing"

  ''' <summary>
  ''' Verifies that the specified object is not null. Default trace level is Error.
  ''' </summary>
  ''' <param name="objectName">The name of the object to test.</param>
  ''' <param name="obj">The object to test for null.</param>
  Public Shared Sub ObjectIsNotNull( _
              ByVal objectName As String, _
              ByVal obj As Object)
    Dim traceLevel As TraceLevel = traceLevel.Error
    If (TSLevel.Level >= traceLevel) AndAlso Trace.Listeners.Count > 0 Then
      If (obj Is Nothing) Then
        ObjectIsNotNullFailed(objectName, _
         traceLevel.Error, obj, New StackFrame(1, True))
      End If
    End If
  End Sub

  ''' <summary>
  ''' Verifies that the specified object is not null. Default trace level is Error.
  ''' </summary>
  ''' <param name="objectName">The name of the object to test.</param>
  ''' <param name="traceLevel">The TraceLevel to use for this trace issue.</param>
  ''' <param name="obj">The object to test for null.</param>
  Public Shared Sub ObjectIsNotNull( _
              ByVal objectName As String, _
              ByVal traceLevel As TraceLevel, _
              ByVal obj As Object)
    If (TSLevel.Level >= traceLevel) AndAlso Trace.Listeners.Count > 0 Then
      If (obj Is Nothing) Then
        ObjectIsNotNullFailed(objectName, _
         traceLevel, obj, New StackFrame(1, True))
      End If
    End If
  End Sub

  Private Shared Sub ObjectIsNotNullFailed( _
              ByVal paramName As String, _
              ByVal traceLevel As TraceLevel, _
              ByVal obj As Object, _
              ByVal stackframe As StackFrame)
    TraceEntry(stackframe, traceLevel, Nothing, paramName, _
                Nothing, TraceIssues.ObjectIsNotNull, _
                "Object " + paramName + " cannot be null")
  End Sub

#End Region

#Region "Integer Less Than"

  ''' <summary>
  ''' Verifies that the specified value is less than the specified maximum. Default trace level is Error.
  ''' </summary>
  ''' <param name="intName">The name of the integer to test.</param>
  ''' <param name="intValue">The integer to compare with the maximum value specified by the HighBound parameter.</param>
  ''' <param name="HighBound">The maximum allowed value of the val parameter.</param>
  Public Shared Sub IntLessThan( _
              ByVal intName As String, _
              ByVal intValue As Int32, _
              ByVal HighBound As Int32)
    Dim traceLevel As TraceLevel = traceLevel.Error
    ' TODO: sanity check for HighBound < highBound
    If (TSLevel.Level >= traceLevel) AndAlso Trace.Listeners.Count > 0 Then
      If (intValue > HighBound) Then
        IntLessThanFailed(intName, _
         traceLevel, intValue, HighBound, _
         New StackFrame(1, True))
      End If
    End If
  End Sub

  ''' <summary>
  ''' Verifies that the specified value is less than the specified maximum. Default trace level is Error.
  ''' </summary>
  ''' <param name="intName">The name of the integer to test.</param>
  ''' <param name="traceLevel">The TraceLevel to use for this trace issue.</param>
  ''' <param name="intValue">The integer to compare with the maximum value specified by the HighBound parameter.</param>
  ''' <param name="HighBound">The maximum allowed value of the val parameter.</param>
  Public Shared Sub IntLessThan( _
              ByVal intName As String, _
              ByVal traceLevel As TraceLevel, _
              ByVal intValue As Int32, _
              ByVal HighBound As Int32)
    ' TODO: sanity check for HighBound < highBound
    If (TSLevel.Level >= traceLevel) AndAlso Trace.Listeners.Count > 0 Then
      If (intValue > HighBound) Then
        IntLessThanFailed(intName, _
         traceLevel, intValue, HighBound, _
         New StackFrame(1, True))
      End If
    End If
  End Sub

  Private Shared Sub IntLessThanFailed( _
              ByVal intName As String, _
              ByVal traceLevel As TraceLevel, _
              ByVal intValue As Int32, _
              ByVal HighBound As Int32, _
              ByVal stackframe As StackFrame)
    TraceEntry(stackframe, traceLevel, intValue, intName, _
              Nothing, TraceIssues.IntLessThan, _
             "Integer " + intName + " must be less than " + _
             HighBound.ToString())
  End Sub

#End Region

#Region "Integer Greater Than"

  ''' <summary>
  ''' Verifies that the specified value is greater than the specified minimum. Default trace level is Error.
  ''' </summary>
  ''' <param name="intName">The name of the integer to test.</param>
  ''' <param name="intValue">The integer to compare with the maximum value specified by the LowBound parameter.</param>
  ''' <param name="LowBound">The minimum allowed value of the val parameter.</param>
  Public Shared Sub IntGreaterThan( _
              ByVal intName As String, _
              ByVal intValue As Int32, _
              ByVal LowBound As Int32)
    Dim traceLevel As TraceLevel = traceLevel.Error
    ' TODO: sanity check for LowBound < LowBound
    If (TSLevel.Level >= traceLevel) AndAlso Trace.Listeners.Count > 0 Then
      If (intValue < LowBound) Then
        IntGreaterThanFailed(intName, _
         traceLevel, intValue, LowBound, _
         New StackFrame(1, True))
      End If
    End If
  End Sub

  ''' <summary>
  ''' Verifies that the specified value is greater than the specified minimum. Default trace level is Error.
  ''' </summary>
  ''' <param name="intName">The name of the integer to test.</param>
  ''' <param name="traceLevel">The TraceLevel to use for this trace issue.</param>
  ''' <param name="intValue">The integer to compare with the maximum value specified by the LowBound parameter.</param>
  ''' <param name="LowBound">The minimum allowed value of the val parameter.</param>
  Public Shared Sub IntGreaterThan( _
              ByVal intName As String, _
              ByVal traceLevel As TraceLevel, _
              ByVal intValue As Int32, _
              ByVal LowBound As Int32)
    ' TODO: sanity check for LowBound < LowBound
    If (TSLevel.Level >= traceLevel) AndAlso Trace.Listeners.Count > 0 Then
      If (intValue < LowBound) Then
        IntGreaterThanFailed(intName, _
         traceLevel, intValue, LowBound, _
         New StackFrame(1, True))
      End If
    End If
  End Sub

  Private Shared Sub IntGreaterThanFailed( _
              ByVal intName As String, _
              ByVal traceLevel As TraceLevel, _
              ByVal intValue As Int32, _
              ByVal LowBound As Int32, _
              ByVal stackframe As StackFrame)
    TraceEntry(stackframe, traceLevel, intValue, intName, _
              Nothing, TraceIssues.IntGreaterThan, _
             "Integer " + intName + " must be greater than " + _
             LowBound.ToString())
  End Sub
#End Region

#Region "Integer In Range"

  ''' <summary>
  ''' Verifies that the specified value is between the range specified by the lowBound and highBound parameters.
  ''' The value is considered to "pass" if it matches one of the parameters (i.e. the range is inclusive of the
  ''' parameters.) Default trace level is Error.
  ''' </summary>
  ''' <param name="intName">The name of the integer to test.</param>
  ''' <param name="intValue">The integer to compare with the lowBound and highBound parameters.</param>
  ''' <param name="lowBound">The minimum allowed value of the val parameter.</param>
  ''' <param name="highBound">The maximum allowed value of the val parameter.</param>
  Public Shared Sub IntInRange( _
              ByVal intName As String, _
              ByVal intValue As Int32, _
              ByVal lowBound As Int32, _
              ByVal highBound As Int32)
    Dim traceLevel As TraceLevel = traceLevel.Error
    ' TODO: sanity check for lowBound < highBound
    If (TSLevel.Level >= traceLevel) AndAlso Trace.Listeners.Count > 0 Then
      If (intValue < lowBound) Then
        IntInRangeFailedLow(intName, _
         traceLevel, intValue, lowBound, highBound, _
         New StackFrame(1, True))
      ElseIf (intValue > highBound) Then
        IntInRangeFailedHigh(intName, _
         traceLevel, intValue, lowBound, highBound, _
         New StackFrame(1, True))
      End If
    End If
  End Sub

  ''' <summary>
  ''' Verifies that the specified value is between the range specified by the lowBound and highBound parameters.
  ''' The value is considered to "pass" if it matches one of the parameters (i.e. the range is inclusive of the
  ''' parameters.) Default trace level is Error.
  ''' </summary>
  ''' <param name="intName">The name of the integer to test.</param>
  ''' <param name="traceLevel">The TraceLevel to use for this trace issue.</param>
  ''' <param name="intValue">The integer to compare with the lowBound and highBound parameters.</param>
  ''' <param name="lowBound">The minimum allowed value of the val parameter.</param>
  ''' <param name="highBound">The maximum allowed value of the val parameter.</param>
  Public Shared Sub IntInRange( _
              ByVal intName As String, _
              ByVal traceLevel As TraceLevel, _
              ByVal intValue As Int32, _
              ByVal lowBound As Int32, _
              ByVal highBound As Int32)
    ' TODO: sanity check for lowBound < highBound
    If (TSLevel.Level >= traceLevel) AndAlso Trace.Listeners.Count > 0 Then
      If (intValue < lowBound) Then
        IntInRangeFailedLow(intName, _
         traceLevel, intValue, lowBound, highBound, _
         New StackFrame(1, True))
      ElseIf (intValue > highBound) Then
        IntInRangeFailedHigh(intName, _
         traceLevel, intValue, lowBound, highBound, _
         New StackFrame(1, True))
      End If
    End If
  End Sub

  Private Shared Sub IntInRangeFailedLow( _
              ByVal intName As String, _
              ByVal traceLevel As TraceLevel, _
              ByVal intValue As Int32, _
              ByVal lowBound As Int32, _
              ByVal highBound As Int32, _
              ByVal stackframe As StackFrame)
    TraceEntry(stackframe, traceLevel, intValue, intName, _
              Nothing, TraceIssues.IntInRange_Low, _
             "Integer " + intName + " must be greater than " + _
             lowBound.ToString())
  End Sub

  Private Shared Sub IntInRangeFailedHigh( _
              ByVal intName As String, _
              ByVal traceLevel As TraceLevel, _
              ByVal intValue As Int32, _
              ByVal lowBound As Int32, _
              ByVal highBound As Int32, _
              ByVal stackframe As StackFrame)
    TraceEntry(stackframe, traceLevel, intValue, intName, _
             Nothing, TraceIssues.IntInRange_Low, _
             "Integer " + intName + " must be less than " + _
             highBound.ToString())
  End Sub
#End Region

#Region "String Required"

  ''' <summary>
  ''' Verifies that the specified string is not null or empty. Default trace level is Error.
  ''' </summary>
  ''' <param name="stringName">The name of the string to test.</param>
  ''' <param name="stringValue">The reference to the string to test.</param>
  Public Shared Sub StringRequired( _
              ByVal stringName As String, _
              ByVal stringValue As String)
    Dim traceLevel As TraceLevel = traceLevel.Error
    If (TSLevel.Level >= traceLevel) AndAlso Trace.Listeners.Count > 0 Then
      If (stringValue Is Nothing OrElse stringValue = "") Then
        StringRequiredFailed(stringName, traceLevel, stringValue, New StackFrame(1, True))
      End If
    End If
  End Sub

  ''' <summary>
  ''' Verifies that the specified string is not null or empty. Default trace level is Error.
  ''' </summary>
  ''' <param name="stringName">The name of the string to test.</param>
  ''' <param name="traceLevel">The TraceLevel to use for this message.</param>
  ''' <param name="stringValue">The reference to the string to test.</param>
  Public Shared Sub StringRequired( _
              ByVal stringName As String, _
              ByVal traceLevel As TraceLevel, _
              ByVal stringValue As String)
    If (TSLevel.Level >= traceLevel) AndAlso Trace.Listeners.Count > 0 Then
      If (String.IsNullOrEmpty(stringValue)) Then
        StringRequiredFailed(stringName, traceLevel, stringValue, New StackFrame(1, True))
      End If
    End If
  End Sub

  Private Shared Sub StringRequiredFailed( _
              ByVal paramName As String, _
              ByVal traceLevel As TraceLevel, _
              ByVal stringValue As String, _
              ByVal stackframe As StackFrame)
    TraceEntry(stackframe, traceLevel, stringValue, paramName, _
              Nothing, TraceIssues.StringRequired, _
             "String " + paramName + " must have a length > 0 ")
  End Sub
#End Region

#Region "Starting Event"

  ''' <summary>
  ''' Records a trace message indicating the beginning of an event. Default trace level is Info.
  ''' </summary>
  ''' <param name="sender">The object that caused the event.</param>
  ''' <param name="e">An EventArgs that contains the event data.</param>
  Public Shared Sub StartingEvent( _
              ByVal sender As System.Object, _
              ByVal e As System.EventArgs)
    Dim traceLevel As TraceLevel = traceLevel.Info
    If (TSLevel.Level >= traceLevel) AndAlso Trace.Listeners.Count > 0 Then
      StartingEvent(sender, e, traceLevel, New StackFrame(1, True))
    End If
  End Sub

  ''' <summary>
  ''' Records a trace message indicating the beginning of an event. Default trace level is Info.
  ''' </summary>
  ''' <param name="sender">The object that caused the event.</param>
  ''' <param name="e">An EventArgs that contains the event data.</param>
  ''' <param name="traceLevel">The TraceLevel to use for this message.</param>
  Public Shared Sub StartingEvent( _
              ByVal sender As System.Object, _
              ByVal e As System.EventArgs, _
              ByVal traceLevel As TraceLevel)
    If (TSLevel.Level >= traceLevel) AndAlso Trace.Listeners.Count > 0 Then
      StartingEvent(sender, e, traceLevel, New StackFrame(1, True))
    End If
  End Sub

  Private Shared Sub StartingEvent( _
              ByVal sender As System.Object, _
              ByVal e As System.EventArgs, _
              ByVal traceLevel As TraceLevel, _
              ByVal stackframe As StackFrame)
    Dim name As String
    Dim properties As String = ""

    ' Roger (2006-08-17): Commented out the following If conditional but allowed the line "name = sender.ToString"
    ' because this assembly will not be used in a WinForms app. Revert if the situation changes.
    'If TypeOf sender Is Windows.forms.Control Then
    '   name = CType(sender, Windows.Forms.Control).Name
    'Else
    name = sender.ToString
    'End If

    If Not TSSkipEventArgs.Enabled Then
      ' Only do the reflection if you know its wanted
      For Each p As Reflection.PropertyInfo In e.GetType.GetProperties
        properties &= p.Name & ":" & p.GetValue(e, Nothing).ToString & ", " & crlf
      Next
    End If
    TraceEntry(stackframe, traceLevel, Nothing, "", _
                Nothing, TraceIssues.StartingMethod, _
                "Starting Event - Called By: " & name & crlf & tab & _
                         "Properties: " & properties)
  End Sub
#End Region

#Region "Starting Method"
  ' This design will cause problems if the first value passed to the method 
  ' is a TraceLevel. I'm ignoring that side case

  ''' <summary>
  ''' Records a trace message indicating the beginning of a method. Default trace level is Info.
  ''' </summary>
  ''' <param name="params">Zero or more objects representing the parameters of the method.</param>
  Public Shared Sub StartingMethod(ByVal ParamArray params() As Object)
    Dim traceLevel As TraceLevel = traceLevel.Info
    If (TSLevel.Level >= traceLevel) AndAlso Trace.Listeners.Count > 0 Then
      StartingMethod(params, traceLevel, New StackFrame(1, True))
    End If
  End Sub

  ''' <summary>
  ''' Records a trace message indicating the beginning of a method. Default trace level is Info.
  ''' </summary>
  ''' <param name="traceLevel">The TraceLevel to use for this message.</param>
  ''' <param name="params">Zero or more objects representing the parameters of the method.</param>
  Public Shared Sub StartingMethod( _
               ByVal traceLevel As TraceLevel, _
               ByVal ParamArray params() As Object)
    If (TSLevel.Level >= traceLevel) AndAlso Trace.Listeners.Count > 0 Then
      StartingMethod(params, traceLevel, New StackFrame(1, True))
    End If
  End Sub

  Private Shared Sub StartingMethod( _
              ByVal params() As Object, _
              ByVal traceLevel As TraceLevel, _
              ByVal stackframe As StackFrame)
    TraceEntry(stackframe, traceLevel, Nothing, "", _
             params, TraceIssues.StartingMethod, _
             "Starting Method ")
  End Sub
#End Region

#Region "In Condition"

  ''' <summary>
  ''' Records a trace message indicating the current execution point is within a conditional (e.g. if..else). Default trace level is Verbose.
  ''' </summary>
  ''' <param name="description">A description of the conditional of the current execution point.</param>
  Public Shared Sub InCondition( _
              ByVal description As String)
    Dim traceLevel As TraceLevel = traceLevel.Verbose
    If (TSLevel.Level >= traceLevel) AndAlso Trace.Listeners.Count > 0 Then
      InCondition(description, traceLevel, New StackFrame(1, True))
    End If
  End Sub

  ''' <summary>
  ''' Records a trace message indicating the current execution point is within a conditional (e.g. if..else). Default trace level is Verbose.
  ''' </summary>
  ''' <param name="description">A description of the conditional of the current execution point.</param>
  ''' <param name="traceLevel">The TraceLevel to use for this message.</param>
  Public Shared Sub InCondition( _
               ByVal description As String, _
               ByVal traceLevel As TraceLevel)
    If (TSLevel.Level >= traceLevel) AndAlso Trace.Listeners.Count > 0 Then
      InCondition(description, traceLevel, New StackFrame(1, True))
    End If
  End Sub

  Private Shared Sub InCondition( _
              ByVal description As String, _
              ByVal traceLevel As TraceLevel, _
              ByVal stackframe As StackFrame)
    TraceEntry(stackframe, traceLevel, Nothing, "", _
                Nothing, TraceIssues.InCondition, _
                "In Condition: " + description)
  End Sub
#End Region

#Region "In For"

  ''' <summary>
  ''' Records a trace message indicating the current execution point is within a for loop (e.g. for, foreach). Default trace level is Verbose.
  ''' </summary>
  ''' <param name="val">The current iteration within the for loop.</param>
  Public Shared Sub InFor( _
              ByVal val As Int32)
    Dim traceLevel As TraceLevel = traceLevel.Verbose
    If (TSLevel.Level >= traceLevel) AndAlso Trace.Listeners.Count > 0 Then
      InFor("In for", val, traceLevel, New StackFrame(1, True))
    End If
  End Sub

  ''' <summary>
  ''' Records a trace message indicating the current execution point is within a for loop (e.g. for, foreach). Default trace level is Verbose.
  ''' </summary>
  ''' <param name="val">The current iteration within the for loop.</param>
  ''' <param name="traceLevel">The TraceLevel to use for this message.</param>
  Public Shared Sub InFor( _
              ByVal val As Int32, _
              ByVal traceLevel As TraceLevel)
    If (TSLevel.Level >= traceLevel) AndAlso Trace.Listeners.Count > 0 Then
      InFor("In for", val, traceLevel, New StackFrame(1, True))
    End If
  End Sub

  ''' <summary>
  ''' Records a trace message indicating the current execution point is within a for loop (e.g. for, foreach). Default trace level is Verbose.
  ''' </summary>
  ''' <param name="description">The description of the for loop.</param>
  ''' <param name="val">The current iteration within the for loop.</param>
  Public Shared Sub InFor( _
                ByVal description As String, _
                ByVal val As Int32)
    Dim traceLevel As TraceLevel = traceLevel.Verbose
    If (TSLevel.Level >= traceLevel) AndAlso Trace.Listeners.Count > 0 Then
      InFor(description, val, traceLevel, New StackFrame(1, True))
    End If
  End Sub

  ''' <summary>
  ''' Records a trace message indicating the current execution point is within a for loop (e.g. for, foreach). Default trace level is Verbose.
  ''' </summary>
  ''' <param name="description">The description of the for loop.</param>
  ''' <param name="val">The current iteration within the for loop.</param>
  ''' <param name="traceLevel">The TraceLevel to use for this message.</param>
  Public Shared Sub InFor( _
              ByVal description As String, _
              ByVal val As Int32, _
              ByVal traceLevel As TraceLevel)
    If (TSLevel.Level >= traceLevel) AndAlso Trace.Listeners.Count > 0 Then
      InFor(description, val, traceLevel, New StackFrame(1, True))
    End If
  End Sub

  Private Shared Sub InFor( _
              ByVal description As String, _
              ByVal val As Int32, _
              ByVal traceLevel As TraceLevel, _
              ByVal stackframe As StackFrame)
    TraceEntry(stackframe, traceLevel, val, "", _
              Nothing, TraceIssues.InFor, _
             "In for: " + description + "(" + val.ToString + ")")
  End Sub
#End Region

#Region "Mark the Spot"

  ''' <summary>
  ''' Records a general-purpose trace message with the desired description. Default trace level is Verbose.
  ''' </summary>
  ''' <param name="description">A string describing the current execution point.</param>
  Public Shared Sub MarkSpot( _
              ByVal description As String)
    Dim traceLevel As TraceLevel = traceLevel.Verbose
    If (TSLevel.Level >= traceLevel) AndAlso Trace.Listeners.Count > 0 Then
      MarkSpot(description, traceLevel, New StackFrame(1, True))
    End If
  End Sub

  ''' <summary>
  ''' Records a general-purpose trace message with the desired description. Default trace level is Verbose.
  ''' </summary>
  ''' <param name="description">A string describing the current execution point.</param>
  ''' <param name="traceLevel">The TraceLevel to use for this message.</param>
  Public Shared Sub MarkSpot( _
              ByVal description As String, _
              ByVal traceLevel As TraceLevel)
    If (TSLevel.Level >= traceLevel) AndAlso Trace.Listeners.Count > 0 Then
      MarkSpot(description, traceLevel, New StackFrame(1, True))
    End If
  End Sub

  Private Shared Sub MarkSpot( _
              ByVal description As String, _
              ByVal traceLevel As TraceLevel, _
              ByVal stackframe As StackFrame)
    TraceEntry(stackframe, traceLevel, Nothing, "", _
              Nothing, TraceIssues.MarkSpot, _
             "Mark Spot:" + description)
  End Sub
#End Region

#End Region

#Region "Define Output "
  Private Shared Sub TraceEntry( _
              ByVal stackFrame As StackFrame, _
              ByVal traceLevel As TraceLevel, _
              ByVal value As Object, _
              ByVal valueName As String, _
              ByVal params() As Object, _
              ByVal traceIssue As TraceIssues, _
              ByVal issue As String)
    Dim traceEntry As TraceEntry
    traceEntry = New TraceEntry(mRunGUID, stackFrame, params, _
                   traceLevel, value, valueName, traceIssue, issue)
    Trace.WriteLine(traceEntry)
  End Sub
#End Region

#Region "Private support routines"
  Private Shared Function ShouldTrace(ByVal traceLevel As TraceLevel) As Boolean
    If (TSLevel.Level >= traceLevel) Then
      Return HasListeners()
    End If
  End Function
#End Region
End Class
