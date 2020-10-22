namespace Radix.Math.Applied.Learning.Reinforced.Testing

// open FSharp.Data
// open FSharp.Json
// open FSharp.Data.HttpRequestHeaders

// module OpenAI =

//     module Gym =

//         module Api =

//             type Environment = JsonProvider<""" {"instance_id":"86d44247"} """>
//             type Observation = JsonProvider<""" {"observation":0} """>
//             type DiscreteSpace = JsonProvider<""" {"info":{"n":2,"name":"Discrete"}} """>
//             type Reaction = JsonProvider<""" {"done":false,"info":{"prob":0.3333333333333333},"observation":1,"reward":0.0} """>



//             module Environment =

//                 type InstantiateEnvironmentCommand = {
//                     env_id: string
//                 }

//                 type ExecuteActionCommand = {
//                     action: int
//                     render: bool
//                 }

//                 let create baseUrl environmentId =
//                     let requestBody = {env_id = environmentId}
//                     let body = TextRequest (Json.serialize requestBody)

//                     let response = Http.RequestString(baseUrl + "/v1/envs/", httpMethod = HttpMethod.Post, headers = [ ContentType HttpContentTypes.Json ], body = body)
//                     Environment.Parse(response)

//                 let reset baseUrl instanceId =
//                     Http.RequestString(baseUrl + "/v1/envs/" + instanceId + "/reset/", httpMethod = HttpMethod.Post, headers = [ ContentType HttpContentTypes.Json ], body = TextRequest "")

//                 let step baseUrl instanceId action render =
//                     let requestBody = {
//                         action = action
//                         render = render
//                     }
//                     let body = TextRequest (Json.serialize requestBody)

//                     Http.RequestString(baseUrl + "/v1/envs/" + instanceId + "/step/", httpMethod = HttpMethod.Post, headers = [ ContentType HttpContentTypes.Json ], body = body)

//             module ActionSpace =
//                 let get baseUrl instanceId =
//                     Http.RequestString(baseUrl + "/v1/envs/" + instanceId + "/action_space/", httpMethod = HttpMethod.Get)

//             module ObservationSpace =
//                 let get baseUrl instanceId =
//                     Http.RequestString(baseUrl + "/v1/envs/" + instanceId + "/observation_space/", httpMethod = HttpMethod.Get)

//             module Monitor =

//                 type StartCommand = {
//                     directory: string
//                     force: bool
//                     resume: bool
//                     video_callable: bool
//                 }

//                 let start baseUrl instanceId outputDirectory clearMonitorFiles resume videoCallable =
//                     let requestBody = {
//                         directory = outputDirectory
//                         force = clearMonitorFiles
//                         resume = resume
//                         video_callable = videoCallable
//                     }
//                     let body = TextRequest (Json.serialize requestBody)

//                     Http.RequestString(baseUrl + "/v1/envs/" + instanceId + "/monitor/start/", httpMethod = HttpMethod.Post, headers = [ ContentType HttpContentTypes.Json ], body = body)

//                 let stop baseUrl instanceId =
//                     Http.RequestString(baseUrl + "/v1/envs/" + instanceId + "/monitor/close/", httpMethod = HttpMethod.Post, headers = [ ContentType HttpContentTypes.Json ], body = TextRequest "")

//         //module Environment =

//         //    module FrozenLake =

//         //        open Radix.Math.Applied.Learning.Reinforced
//         //        open Radix.Math.Applied.Probability.Sampling

//         //        type Step = JsonProvider<""" {"done":false,"info":{"prob":0.3333333333333333},"observation":4,"reward":0.0} """>

//         //        let create baseUrl discount render =
//         //            let frozenLake = Api.Environment.create baseUrl "FrozenLake-v0"

//         //            {
//         //                Dynamics = fun (Action action) ->

//         //                                let observation = Step.Parse(Api.Environment.step baseUrl frozenLake.InstanceId action render)
//         //                                Randomized (State observation.Observation, Reward (float observation.Reward), observation.Done)
//         //                Discount = discount
//         //                Actions = [0 .. 3] |> List.map (fun a -> Action a)
//         //                Observations = [0 .. 15] |> List.map (fun o -> Observation o)
//         //            }


