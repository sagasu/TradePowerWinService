# Initial thoughts about `Windows Service` as a project type    
I implemented a solution as a `windows service` just as it was requested in the requirements. It is worth pointing that if it was a real life project I would strongly doing it by using:  
* [Azure Function](https://learn.microsoft.com/en-us/azure/azure-functions/functions-overview), or even better [Durable Azure Function](https://learn.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-overview?tabs=csharp-inproc) that would give us better observability over job status w/o a need of introducing plenty of boilerplate code that happens when we use `Windows Service`  
* Docker running in K8s - If I don't want to vendor lock I can just implement a simple C# library or Console application that runs in Docker container and dedicate triggering it (scheduling) to K8s  
* [Hangfire](https://www.hangfire.io/) scheduler job. I would consider using Hellfire or similar schedule only if Azure Function or K8s solution is not an option. Still using Hellfire is much better in long run (maintainability, simplicity, observability) then implementing old school `Windows Service`  

# How to improve the code base even more, next steps.  
* Not referencing `PowerService.dll` directly, wrapping it in Nuget Package and importing it as a NuGet dependency.  
* Adding more observability to the project, ideally Open Telemetry, especially tracability, thanks to that we can reduce the number of log calls in app. If possible use App Insights, they are brilliant and easy to configure.  
* If project is going to grow I recommend extracting `Services` folder and `Models` folder to external projects. As it is implemented now I feel like project is too small for that and I recommend keeping it all together.  