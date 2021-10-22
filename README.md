# Virtual Coffee Morning Assigner

With everyone working remotely at the moment, we have virtual coffee mornings once a week where we divide people into random groups of four to have a coffee and chat together over a virtual call.

Assigning users to groups to avoid repeat meetings is an NP-hard problem. 
This problem is also known as the Social Golfer problem and is well researched and documented: 
https://en.wikipedia.org/wiki/Social_golfer_problem

This console application implements a Genetic Algorithm to process a list of users from a CSV file
and assigns them into new groups of four.

![image](https://user-images.githubusercontent.com/1926575/138348972-d3d90239-b5a9-4f3b-8606-876cb70cc3b3.png)

The fitness function looks at when people have met previosuly and more weight is given to recent meetings. This skewed penalty weighting helps drive the algorithm towards picking solutions where no-one has met recently.

Each user is represented as a Gene and the user group assignments are represented as Chromosomes. The gene position in the Chromosome determines which group a user is assigned to. Positions 0 - 3 map to group 1, positions 4 - 7 map to group 2 and so on.

## Fitness
How do we evaluate the fitness of each chromosome? We want to minimise the number of repeat meetings each week and if there are repeat meetings, we'd like the algorithm to favour solutions that had repeat meetings long ago over solutions that had repeat meetings recently. 

To do this, we read in the history of all the previous group assignments for the last n weeks and we create a penalty score matrix for all users that have met previously. If two users met n weeks ago, they get 1 penalty point. If they met n-1 weeks ago, they get 2 penalty points. If they met last week, they get n-1 penalty points.

Once we calculate the penalty points for all 80 users for the last 20 weeks (80 x 20 calculations), we have a lookup matrix for our fitness function.

The fitness function is applied to each Chromosome solution. For each group we look at all the pairs of people and lookup their penalty score. We aggregate all the combinations for all users in all groups in each Chromosome and assign the combined penalty score to that Chromosome.

## Evolving
The algorithm is then able to compare the fitness of two Chromosomes using the Fitness function.

To "breed" new children, we take two parents and select two random points along the Chromosome in one parent. The genes between the points are passed to the child. The remaining genes are transferred from the same parent, but in the order that they appear in the second parent. The result is that the child contains all of the values from a single parent but includes ordering, and therefore traits, from both parents and avoids the possibility of duplicates or exclusions.

We also encourage the best solutions to pass through to the next generation by keeping a percentage of the Chromosomes unchanged. These are referred to as Elite chromosomes.

Finally, to introduce some diversity into the population we introduce mutations. We randomly change the order of Genes in some of the Chromosomes. This helps the algorithm to avoid local maxima and to converge more quickly to a more optimal global solution.

![image](https://user-images.githubusercontent.com/1926575/138349133-c72b146a-bc58-499a-8a0a-f7a92abd8bee.png)

