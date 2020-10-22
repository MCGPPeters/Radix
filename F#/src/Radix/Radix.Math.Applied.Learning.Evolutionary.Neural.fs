namespace Radix.Math.Applied.Learning.Evolutionary.Neural

// open Radix.Math.Applied.Learning.Neural
// open Radix.Math.Applied.Probability
// open Radix.Math.Applied.Learning.Evolutionary
// open Radix.Math.Applied.Learning.Evolutionary.Recombination
// open Radix.Math.Applied.Probability.Distribution
// open Radix.Math.Applied.Probability.Sampling
// open Radix.Collections.List
// open Radix.Collections
// open MassTransit

// type ConnectionGene = Gene<Connection>

// module Neat =

//     let crossover (mom:  Worthiness<Mate<'TGene, 'TMerit> >) (dad: Worthiness<Mate<'TGene, 'TMerit>>) =
//         match (mom, dad) with
//         | (Worthy mom, Worthy dad) | (Less mom, Less dad) ->
//           allign mom.Chromosome.Genes dad.Chromosome.Genes (fun gene -> gene.Locus)
//           |> List.map (fun genePair ->
//                         match genePair with
//                         // Genes on the same locus get picked at random from the 2 parents
//                         | (Some mom, Some dad) ->
//                             let (Randomized mate) = uniform(List(mom, [dad])) |> pick
//                             Some mate
//                         | (_, dad) -> dad)
//         | (Worthy mom, Less dad) ->
//           allign mom.Chromosome.Genes dad.Chromosome.Genes (fun gene -> gene.Locus)
//           |> List.map (fun genePair ->
//                             match genePair with
//                             // Genes on the same locus get picked at random from the 2 parents
//                             | (Some mom, Some dad) ->
//                                 let (Randomized mate) = uniform(List(mom, [dad])) |> pick
//                                 Some mate
//                             | (mom, _) -> mom)
//         | (Less mom, Worthy dad) ->
//           allign mom.Chromosome.Genes dad.Chromosome.Genes (fun gene -> gene.Locus)
//           |> List.map (fun genes ->
//                             match genes with
//                             // Genes on the same locus get picked at random from the 2 parents
//                             | (Some mom, Some dad) ->
//                                 let (Randomized mate) = uniform(List(mom, [dad])) |> pick
//                                 Some mate
//                             | (_, dad) -> dad)

//     // For every point in each Genome, where each Genome shares
//     // the innovation number, the Gene is chosen randomly from
//     // either parent.  If one parent has an innovation absent in
//     // the other, the baby may inherit the innovation
//     // if it is from the more fit parent.
//     let recombine (wortiness: Recombination.Worthiness<Connection, 'TMerit>) : Recombination<Connection, 'TMerit> =
//         (fun (Parent mom) (Parent dad) ->
//             let (mom, dad) = wortiness mom dad
//             let offspring = crossover mom dad |> List.choose id
//             {Id = ChromosomeId Sampling.Guid; Genes = offspring})

//     module Mutation =
//         let alterWeight connection weightDistribution =
//             let (Randomized weight) = weightDistribution |> pick
//             {connection with Weight = weight}

//         let addConnection connection chromosome =
//             let genes = List.append [{ Locus = Locus (NewId.Next()); Id = GeneId Sampling.Guid; Allele = connection; Enabled = true }] chromosome.Genes
//             { chromosome with Genes = genes }

//         let rec disable (con: Connection) (genes: Gene<Connection> list) =
//                 match genes with
//                 | [] -> []
//                 | x::xs when x.Allele.Id = con.Id -> { x with Enabled = false } :: (disable con xs)
//                 | x::xs -> x :: disable con xs

//         let addNode node connection (chromosome: Chromosome<Connection>) =
//             let incomingConnection =
//                 {
//                     connection with
//                         Id = ConnectionId Sampling.Guid
//                         Input = connection.Input
//                         Output = node
//                         Weight = 1.0
//                 }
//             let outgoingConnection =
//                 {
//                     connection with
//                         Id = ConnectionId Sampling.Guid
//                         Input = node
//                 }

//             let genes = chromosome.Genes
//                             |> disable connection
//                             |> List.append [{ Locus = Locus (NewId.Next()); Id = GeneId Sampling.Guid; Allele = incomingConnection; Enabled = true }]
//                             |> List.append [{ Locus = Locus (NewId.Next()); Id = GeneId Sampling.Guid; Allele = outgoingConnection; Enabled = true }]

//             { chromosome with Genes = genes }

//     // module Benefit =

//     //     let novelty: Novelty<'a> = fun event ->

//     module Speciation =

//         /// I'm ignoring the difference between disjoint and excess genes
//         let distance differenceCoefficient averageWeightDifferenceCoefficient (c1: Chromosome<Connection>) (c2: Chromosome<Connection>) =
//             let (nGenes, nDifferent, avgWeightDifference) = allign c1.Genes c2.Genes (fun gene -> gene.Locus)
//                                                             |> List.fold (fun acc (gene1, gene2) ->
//                                                                                 let (n, nDifferent, averageWeightDifference) = acc
//                                                                                 let n' = n+1
//                                                                                 let alpha = 1.0/(float n')
//                                                                                 let (nMismatched', target) = match gene1, gene2 with
//                                                                                                                 | Some x, Some y -> (nDifferent, abs (x.Allele.Weight - y.Allele.Weight))
//                                                                                                                 | _ -> (nDifferent + 1, averageWeightDifference)
//                                                                                 let error = target - averageWeightDifference
//                                                                                 let avgWeight' = averageWeightDifference + alpha * error
//                                                                                 (n', nMismatched', avgWeight')) (0, 0, 0.0)
//             (differenceCoefficient * (float (nDifferent / nGenes))) * (averageWeightDifferenceCoefficient * avgWeightDifference)
