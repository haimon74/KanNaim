
using System;
using System.Collections.Generic;
using System.Workflow.Runtime.Hosting;
using System.Workflow.Runtime;
using System.Web;
using System.Diagnostics;


namespace DashboardBusiness
{
    using Exceptions;

	public static class WorkflowHelper
	{
        public static void ExecuteWorkflow( Type workflowType, Dictionary<string,object> properties)
        {
            WorkflowRuntime workflowRuntime = Init();
        
            ManualWorkflowSchedulerService manualScheduler = workflowRuntime.GetService<ManualWorkflowSchedulerService>();
        
            WorkflowInstance instance = workflowRuntime.CreateWorkflow(workflowType, properties);        
            instance.Start();

            EventHandler<WorkflowCompletedEventArgs> completedHandler = null;
            completedHandler = delegate(object o, WorkflowCompletedEventArgs e)
            {
                if (e.WorkflowInstance.InstanceId ==instance.InstanceId)
                {
                    workflowRuntime.WorkflowCompleted -= completedHandler;
                    
                    // copy the output parameters in the specified properties dictionary
                    Dictionary<string,object>.Enumerator enumerator = e.OutputParameters.GetEnumerator();
                    while( enumerator.MoveNext() )
                    {
                        KeyValuePair<string,object> pair = enumerator.Current;
                        if( properties.ContainsKey(pair.Key) )
                        {
                            properties[pair.Key] = pair.Value;
                        }
                    }
                }
            };

            Exception x  = null;
            EventHandler<WorkflowTerminatedEventArgs> terminatedHandler = null;            
            terminatedHandler = delegate(object o, WorkflowTerminatedEventArgs e)
            {
                if (e.WorkflowInstance.InstanceId == instance.InstanceId)
                {
                    workflowRuntime.WorkflowTerminated -= terminatedHandler;                    
                    Debug.WriteLine( e.Exception );

                    x = e.Exception;
                }
            };

            workflowRuntime.WorkflowCompleted += completedHandler;
            workflowRuntime.WorkflowTerminated += terminatedHandler;
            
            manualScheduler.RunWorkflow(instance.InstanceId);

            if (null != x)
                throw new WorkflowException(x);
        }
        
        public static WorkflowRuntime Init()
        {
            WorkflowRuntime workflowRuntime;

            // Running in local mode, create an return new runtime
            if( HttpContext.Current == null )
                workflowRuntime = new WorkflowRuntime();
            else
            {
                // running in web mode, runtime is initialized only once per 
                // application
                if( HttpContext.Current.Application["WorkflowRuntime"] == null )
                    workflowRuntime = new WorkflowRuntime();
                else
                    return HttpContext.Current.Application["WorkflowRuntime"] as WorkflowRuntime;
            }   

            var manualService = new ManualWorkflowSchedulerService();
            workflowRuntime.AddService(manualService);
            
            var syncCallService = new Activities.CallWorkflowService();
            workflowRuntime.AddService(syncCallService);

            workflowRuntime.StartRuntime();

            // on web mode, store the runtime in application context so that
            // it is initialized only once. On dekstop mode, ignore
            if( null != HttpContext.Current )
                HttpContext.Current.Application["WorkflowRuntime"] = workflowRuntime;

            return workflowRuntime;
        }

        public static void Terminate()
        {
            if (HttpContext.Current == null) return;

            WorkflowRuntime workflowRuntime = HttpContext.Current.Application["WorkflowRuntime"] as System.Workflow.Runtime.WorkflowRuntime;
            workflowRuntime.StopRuntime();

            HttpContext.Current.Application.Remove("WorkflowRuntime");
        }
	}
}
