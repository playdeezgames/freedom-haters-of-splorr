# Playtest notes (Claude's pass, before the owner's)

Method: throwaway simulations over generated galaxies (seeds 1-12, default settings, since deleted; the numbers below
are what they printed), plus a short play of a fresh game in the browser pane. Nothing here has been fixed.

## Broken (changes how the game plays)

1. **Trading is an infinite money machine.** A bulk buy or sell is priced at one flat unit price, and your own trades
   only move the price afterwards (1% per 5 units, fading 25% per tick). Planets in the *same* star system differ by
   2-3x on the same good (e.g. Gems 68 vs 161, Narcotics 38 vs 124, Hype 10 vs 22), so a bot that buys all it can at
   one planet and sells at the next, no travel beyond the system, grew 1,000 jools to about 500,000 in 10 hops, and
   hundreds of millions in 60. The weight rule limits a hop to a few hundred units (fuel per move = 1 + weight/25 on a
   250 tank), which still gives tens of thousands of jools a hop once you have capital. All gear together costs
   about 5,750 per mark-V set. After a handful of hops money means nothing, and there is no goal that spends it.
2. **Patrols make interstellar travel ruinous.** Ships are about 25 on 3,969 galaxy cells with a 6-cell chase range
   and 75-90% of them hostile to SIGMO. A bot flying 60 random trips of about 60 moves between systems got 2.0-2.8
   shakedowns per trip and paid 340-460 jools per trip, from a 1,000 start. A random walker met a ship in 36 of 40
   flights of 150 moves, median first contact at move 8-17. Fines are 25% of your jools, so an errand paying about
   51 jools (see 4) loses money, and a fresh player is poor within three trips. In-system flying has no patrols
   at all, so the profitable play is to never leave a system, which is also the trade exploit in 1.
3. **A new game can open with a shakedown.** 41% of 200 fresh seeds start with a hostile ship inside chase range,
   2% start with one adjacent (HALT on turn 1). In the browser my first run was fined 240 of 960 jools on turn 4.
4. **Errands pay almost nothing for their length.** Reward is 5d20 whatever the distance: average 51 jools for an
   average 39-cell trip, about 0.9 jools per move after fuel (worst -0.2, best 18). Trade pays 20-120 per cell.

## Bad balance / traps

5. **Resist with no weapon is a guaranteed loss** (0% wins at any enemy tech over 200 trials each), yet it is always
   offered. Laser I alone wins only against tech 0-3 ships (and ends at 13 hull against tech 3). Laser I + Shield I +
   Plating I wins 96% against tech 6 and 0% against tech 10. Laser III + Shield II + Plating II wins everything.
   Fights are close to deterministic (damage varies by 1d4 only), so they are cliffs, not odds.
6. **Gear prices vs income:** Laser I 300 + Shield I 250 + Plating I 200 is about 750, a whole middle-class start.
   The set that beats everything is 1,800+. Errands cannot pay for it; only the trade exploit can.
7. **Killing a ship pays nothing.** The wreck is 6d6 scrap (1 jool each, about 21), against the reputation hit
   (-5 faction and planet) and the respawn in 100 turns. Combat is all downside except infamy.
8. **You cannot start a fight.** Bumping a ship offers only Cancel; there is no Attack, so the "player attacks first"
   case from the design was never built.
9. **Rough planets swing from 47 to 127 of about 290** depending on how many planets a low-Authority/Standards
   faction gets, so black markets and shady jobs are everywhere in some galaxies and thin in others.
10. **Weight is stepwise and cliff-like:** 24 units cost nothing extra, 25 cost 1 fuel a move; Metal (weight 4) at
    100 units burns 17 fuel a move, so bulk goods are unusable on a Mark I tank. Heavy loads past about 6,000 weight
    cost more fuel than the whole tank, so you get one move per tank (the move is clamped, never blocked).
11. **Very Poor starts with 0 jools and a -999 bankruptcy line** (from the VB), so the first shakedown fine is the
    minimum 10 and a distress refuel (250 jools for an empty tank, 1 per fuel) walks you toward -999 fast.
12. **Hostility is nearly universal** (18-24 of 25 ships hostile at the start), which is the satire, but it means
    friendly contact (hails) is rare: 5-10 hails against 86-130 shakedowns in the random-walk runs.

## Smaller oddities

13. The contact screen's quote wraps onto two cramped rows (the font has no line gap); the combat screen was spaced
    out afterwards but the contact one was not.
14. Small goods round hard: Metal reads 5/3 buy/sell on a planet where the market price is 4, so the spread is far
    above the stated 10% for cheap goods (Oxygen 8/6, Fuel 5/3).
15. No slippage inside one transaction also means "Buy As Many As Possible" has the same unit price for 1 or 1,000.
16. The home system was 8 to 75 cells from the start in the seeds tried, so the first errands home are long trips
    through patrol space.
17. Dense galaxies save at about 400 KB; six dense slots (base64 in localStorage) approach browser limits.
18. In the browser pane the first screenshot after a key press often shows the previous frame (a capture lag, not the
    game), so scripted playthroughs need a wait.

## What looked fine

- Moving costs, the tank (250 cells for 84 jools at a dock), and oxygen are comfortable; the map and orbit screens,
  the contact, combat, status and goods screens all read clearly at 40 columns.
- Friendly SIGMO ships hail rather than fine you; shakedown and search flows, saving, and the pedia behaved.

## Suggested fixes, in order of impact

1. Slippage: price each unit of a bulk trade at the pressure it creates (or a per-market stock limit), and narrow the
   spread between planets in one system (smaller trait percentages, or a bigger gap cost for hauling).
2. Start the avatar at least 8 cells from any ship; cut the ship count to about one per five systems, or make fines
   scale with the trip (once per ship and only hostile ones within 3 cells), and let a bribe be cheaper than 25%.
3. Scale errand rewards with distance, and let kills drop real loot (sellable parts) so fighting can pay.
4. Hide Resist when unarmed (or make it a real gamble), and add Attack on a bumped ship.

## After the fixes (same bots, same seeds)

- Flying 60 trips of about 55 moves: 0.82-0.92 shakedowns a trip (was 2.0-2.8) and 72-86 jools of fines a trip (was
  340-460).
- Hauling inside one system for 30 hops: 1,000 jools became 1,000-1,430 (was hundreds of millions). The best haul
  across the whole galaxy is +23% to +47% before slippage (spread already included), so trade is a modest living.
- Errands: average reward 120 for a 38-cell trip, 3.1 jools a move (was 0.9).
- New games: 0 of 200 start with a hostile ship in chase range (was 83).
- Resist is hidden unarmed; Attack exists; kills drop 2-4 (+tech/4) Ship Parts at 30 jools plus sometimes cargo; gear is
  100 x mark squared (Mark V 2,500); weight is smooth (weight/50 fuel a move).
