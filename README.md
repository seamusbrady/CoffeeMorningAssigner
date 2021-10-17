# CoffeeMorningAssigner

Console application to take a list of users
and randomly divide them into groups of 4
for a virtual coffee morning.

Each week, the people will be put into
new groups and the program should reduce
the likelihood of people meeting each other again.


With 75 users there are about 74!/3! 
55131424025323106870992171370208963690947208636822495195101987805901872616828594243208478720000000000000000 
possible permutations so checking all possible
combinations is not feasible.

The app uses a genetic algorithm to generate a 
viable solution that minimises the number of repeat meetings
With more weight given to recent meetings. 

