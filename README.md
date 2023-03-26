# Initial thoughts about `Windows Service` as a project type    
I implemented a solution as a `windows service` just as it was requested in the requirements. It is worth pointing that if it was a real life project I strongly recommend taking a different approach:  
* [Azure Function](https://learn.microsoft.com/en-us/azure/azure-functions/functions-overview), or even better [Durable Azure Function](https://learn.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-overview?tabs=csharp-inproc) that would give us better observability over job status w/o a need of introducing plenty of boilerplate code that happens when we use `Windows Service`  
* Docker running in K8s - If I don't want to vendor lock I can just implement a simple `C# library` or `Console application` that runs in Docker container and delegate triggering it (scheduling) to K8s  
* [Hangfire](https://www.hangfire.io/) scheduler job. I would consider using Hellfire or similar schedule only if Azure Function or K8s solution is not an option. Still using Hellfire is much better in long run (maintainability, simplicity, observability) then implementing old school `Windows Service`  

# Few words about codebase
* It is a code smell to close bind your application to external library model, to address this smell I implement a DTO layer so the code base is agnostic from `PowerService.dll`. I am usually a huge fan of not ever binding to external libriary models, because it is really hard to change app when a model in dll changes.
* I implemented `date time service pattern` it is a common approach if you want to unit test application and have a full control over date. It helps in making sure that time across entire application is this same, and it is easy to control it. Makes it easier to change from local time to UTC if needed.
* I access variables from appsetings thru `Service Config`. I am a huge fan of strong types, and this is a common pattern to be able to use appsettings maintain strong types, and not using `magic strings` in your app.
* For error handling I catch all exceptions in TimedService and return `Environment.Exit(1)` to ask Windows to restart the entire service for me. It also means that after service is restarted it will try to run the task again. If you run this app from Visual Studio and not as a Service then of course Windows is not going to restart the service, in this case in case of exception your debugging session will end. This is by design. Again this solution is a `Windows Service` designed to run as a `Windows Service` and not something else.
* 


# It is not acceptable to miss a scheduled extract.  
I am not sure If I understand this requirement. I did not implement any complex `retry mechanism`  or logic to check when was the last time job succeeded an carry on from that place. My understanding is that this project is already too large for a simple task, that is why I assume that implementing such mechanisms is outside a scope of the project. Again it would be easier to do it if it was `Azure Function`, `K8s` or `Hangfire job`. Please let me know if you want me to add this functionality. The way how it works now is in case of error entire Windows Service will be restarted which will trigger the execution of job again.  

# How to improve the code base and next steps.  
* Not referencing `PowerService.dll` directly, wrapping it in Nuget Package and importing it as a NuGet dependency.  
* Adding more observability to the project, ideally Open Telemetry, especially tracability, thanks to that we can reduce the number of log calls in app. If possible use App Insights, they are brilliant and easy to configure.  
* If project is going to grow I recommend extracting `Services` folder and `Models` folder to external projects. As it is implemented now I feel like project is too small for that and I recommend keeping it all together.  