/*************************************************************
* Usage:
* //Rolls 4 d6 and returns a list each die value
* DiceUtility.RollDice(6, 4);
*
* //Rolls 4 d6 and drops the lowerst and returns total value of the 3 remaining
* DiceUtility.SumDie(DiceUtility.RollDice(6, 4), 1); 
*
* //Rolls 4 d6 and returns total value
* DiceUtility.SumDie(DiceUtility.RollDice(6, 4), 0); 
*
* var die = 6; //Die size
* var numRoll = 4; //Number of Dice
* var dropNumLowest = 1 //Drop lowest 2 = two lowest etc... 
* DiceUtility.SumDie(DiceUtility.RollDice(die, numRoll), dropNumLowest)); 
*
*************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DiceUtility {

  public static List<int> RollDice(int die, int count) {
    List<int> dierolls = new List<int>();
    
    for(var i = 0; i < count; ++i) {
      int roll = getRandomInt(1, die);
      dierolls.Add(roll);
    }
    dierolls.Sort();
    return dierolls;
  }

  public static int SumDie(List<int> rolls, int dropCount = 0) {
    int total = 0;

    for(var i = dropCount; i < rolls.Count; ++i){
      total += (rolls[i]);
    }

    return total;
  }

	public static int getRandomInt(int min, int max) {
		int value = UnityEngine.Random.Range(min, max+1);
    return value;
  }
}

