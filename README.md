# Initial thoughts about `Windows Service` as a project type    
I implemented a solution as a `windows service` just as it was requested in the requirements. It is worth pointing that if it was a real life project I strongly recommend taking a different approach:  
* [Azure Function](https://learn.microsoft.com/en-us/azure/azure-functions/functions-overview), or even better [Durable Azure Function](https://learn.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-overview?tabs=csharp-inproc) that would give us better observability over job status w/o a need of introducing plenty of boilerplate code that happens when we use `Windows Service`  
* Docker running in K8s - If I don't want to vendor lock I can just implement a simple `C# library` or `Console application` that runs in Docker container and delegate triggering it (scheduling) to K8s  
* [Hangfire](https://www.hangfire.io/) scheduler job. I would consider using Hellfire or similar schedule only if Azure Function or K8s solution is not an option. Still using Hellfire is much better in long run (maintainability, simplicity, observability) then implementing old school `Windows Service`  

# It is not acceptable to miss a scheduled extract.  
I am not sure If I understand this requirement. I did not implement any `retry mechanism`  or reset web service in case it fails, or notification system in case of error. My understanding is that this project is already too large for a simple task, that is why I assume that implementing such mechanisms is outside a scope of the project. Again it would be easier to do it if it was `Azure Function`, `K8s` or `Hangfire job`. Please let me know if you want me to add this functionality.

# How to improve the code base and next steps.  
* Not referencing `PowerService.dll` directly, wrapping it in Nuget Package and importing it as a NuGet dependency.  
* Adding more observability to the project, ideally Open Telemetry, especially tracability, thanks to that we can reduce the number of log calls in app. If possible use App Insights, they are brilliant and easy to configure.  
* If project is going to grow I recommend extracting `Services` folder and `Models` folder to external projects. As it is implemented now I feel like project is too small for that and I recommend keeping it all together.  