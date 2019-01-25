#r @"c:\dev\.nuget\fsharp.data\3.0.0\lib\netstandard2.0\FSharp.Data.dll"
#r @"c:\dev\.nuget\fsharp.json\0.3.4\lib\netstandard2.0\FSharp.Json.dll"
#r @"c:\dev\.nuget\greenpipes\2.1.3\lib\netstandard2.0\GreenPipes.dll"
#r @"c:\dev\.nuget\masstransit\5.2.1\lib\netstandard2.0\MassTransit.dll"
#load @"c:\dev\Radix\F#\src\Radix\Radix.fs"
#load @"c:\dev\Radix\F#\src\Radix\Radix.Math.Pure.Algebra.Structure.fs"
#load @"c:\dev\Radix\F#\src\Radix\Root.fs"
#load @"c:\dev\Radix\F#\src\Radix\Radix.Collections.List.fs"
#load @"c:\dev\Radix\F#\src\Radix\Radix.Collections.NonEmpty.fs"
#load @"c:\dev\Radix\F#\src\Radix\Radix.Math.Pure.Structure.Order.fs"
#load @"c:\dev\Radix\F#\src\Radix\Radix.Math.Pure.Numbers.fs"
#load @"c:\dev\Radix\F#\src\Radix\Radix.Math.Applied.Optimization.fs"
#load @"c:\dev\Radix\F#\src\Radix\Radix.Math.Applied.Learning.fs"
#load @"c:\dev\Radix\F#\src\Radix\Radix.Math.Applied.Probability.fs"
#load @"c:\dev\Radix\F#\src\Radix\Radix.Math.Applied.Learning.Evolutionary.fs"
#load @"c:\dev\Radix\F#\src\Radix\Radix.Math.Applied.Learning.Reinforced.fs"
#load @"c:\dev\Radix\F#\src\Radix\Radix.Math.Applied.Learning.Reinforced.Testing.fs"
#load @"c:\dev\Radix\F#\src\Radix\Radix.Math.Applied.Learning.Neural.fs"
#load @"c:\dev\Radix\F#\src\Radix\Radix.Math.Applied.Learning.Evolutionary.Neural.fs"


open Radix.Math.Applied.Learning.Reinforced.Testing.OpenAI.Gym.Api

let baseUrl = "http://localhost:5000"

let environmentId = "FrozenLake-v0"

let environment = Environment.create baseUrl environmentId
ActionSpace.get baseUrl environment.InstanceId

ObservationSpace.get baseUrl environment.InstanceId




Monitor.start baseUrl environment.InstanceId "c:\\tmp\\gym" true false false

Environment.reset baseUrl environment.InstanceId
Environment.step baseUrl environment.InstanceId 3 true
Environment.step baseUrl environment.InstanceId 3 true
Environment.step baseUrl environment.InstanceId 0 true
Environment.step baseUrl environment.InstanceId 0 true
Environment.step baseUrl environment.InstanceId 1 true
Environment.step baseUrl environment.InstanceId 1 true
Environment.step baseUrl environment.InstanceId 2 true
Environment.step baseUrl environment.InstanceId 2 true

Monitor.stop baseUrl environment.InstanceId