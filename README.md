# PlantsVsZombies-Unity
A Unity version of PvZ. Still working on it! 

*Unity version: 2022.3*

Here are some things I've already implemented. Currently there are 6 plants to use, as well as some unique features (part of which are inspired by PvZ 2). 

## Unique Features

### 1. A shovel that recycles sun when removing plants! 

Usually the shovel recycles 50% of the sun cost, provided that the cost is no less than 50 sun value. Note that **Single-use plants offer no sun payback when removed.**

### 2. Boost the plants! 

Well... In fact it's the cells that are boosted. Just click on the sun icon and you would get a *boost sun* (appears red), which you can place in any cell that is not currently boosted. The cost is 100 sun value per cell. The boost effect will apply once a plant is set in the cell (if there is already one, of course the boost takes effect immediately). **Each plant can only be boosted once. Boost effect is revoked if you shovel the plant, with no additional sun recycled.** By default, every plant restores full health when boosted. 

### 3. Automatically collect sun! 

I'm lazy...in many aspects. First, the mechanism that allows manually clicking on a sun and collect it causes endless troubles relating to all those colliders. Second, I'm too lazy to collect the sun. Solution? The sun will go where they should go once they are dropped. 

## Classic Plants

### 1. Sunflower

The classic plant that produces sun for you. 

- Sun cost: 50
- Cooldown time: 4 seconds
- Max health: 100
- Sun payback on shoveled: 25
- Sun production interval: 12 seconds
- Boost effect: *Immediately produce 125 sun value; reduce production interval to 9 seconds since boosted*

### 2. Jalapeno

Burns a whole row of zombies. Always reliable if a huge wave of zombies appear! 

- Sun cost: 125
- Cooldown time: 8 seconds
- Max health: 2147483647 (all single-use plants are invicible when exploding, of course)
- Sun payback on shoveled: 0
- Damage: Burn zombies on the same row
- Boost effect: *Burn 3 adjacent rows of zombies instead of 1 row*

### 3. Cherry Bomb

The best companion of Jalapeno! 

- Sun cost: 150
- Cooldown time: 8 seconds
- Max health: 2147483647
- Sun payback on shoveled: 0
- Damage: Effective within a 3x3 square (approximately; the actual range is a circle)
- Boost effect: *1.5x Explosion radius (expand to about 5x5 square; basically a doom-shroom which does no harm to your lawn!)*

### 4. Potato Mine

They are really cheap! 

- Sun cost: 25
- Cooldown time: 7 seconds
- Activation time: 10 seconds
- Max health: 100 ...when not active; when active, why care about its health?
- Sun payback on shoveled: 0
- Damage: Effective within 3 cells (1 left and 1 right)
- Boost effect: *1.2x Explosion radius and damages zombies on adjacent rows*

### 5. Wallnut

The classic defender. Works best with a potato mine behind it! 

- Sun cost: 50
- Cooldown time: 12 seconds
- Max health: 1500
- Sun payback on shoveled: 25
- Boost effect: *2x Max health and immediately restore to full health (namely, 3000)*
- Switch to damage stage 1 when health percentage is below 65%, and damage stage 2 when below 30%

### 6. Peashooter

The most classic attacker. Now equipped with boost ability!

- Sun cost: 100
- Cooldown Time: 6 seconds
- Max health: 100
- Sun payback on shoveled: 50
- Shoot interval: 1.5 seconds; only shoot when there are zombies in the row
- Damage: 10 **(unboosted)**; 12 **(boosted)**; *10 unboosted peas will kill an ordinary zombie*
- Boost effect: *Immediately shoot 40 peas; 0.8x shoot interval (namely, to 1.2 seconds); 1.5x pea bullet speed; 1.2x single bullet damage; shoot 2x peas at a time from then on*

## Zombies

Currently there is only *one* kind of zombie: the most ordinary **ordinary zombie.** Here are some basic info. 

- Max health: 100
- Attack: 25 damage per second
- Move speed: *ORDINARY* (I don't know how to describe this; to be technical, they move at a random speed of 0.9-1.3 units per second. This can be changed at any time.)
- Are they resistent to any single-use plant? **NO.**

## Coming Soon...

- Constructed zombie health system. Future features: two derived Peashooters, namely *Repeater* and *Gatlin Pea*; and two tougher zombies, namely *Conehead zombie* and *Buckethead zombie*. 

- More game contents, such as level progress and level selector, will... Well, I don't know when they can be published. Maybe after a long time. 
