﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class Game
    {
        #region fields and properties
        private CupOfDice cup;
        private Player playerOne;
        public int RoundNumber { get; private set; }
        private int[] diceValues = new int[Rulebook.AMOUNT_OF_DICE];
        public bool RoundIsInProgress { get; private set; }
        public bool GameHasEnded { get; private set; }
        #endregion

        #region Constructors
        public Game()
        {
            cup = new CupOfDice(Rulebook.AMOUNT_OF_DICE, Rulebook.MAX_DICE_VALUE);
            playerOne = new Player();
            StartNewTurn();
        }
        #endregion

        #region methods
        public void EvaluateRoll(bool[] values, int chosenCombination)
        {
            //Check if game ended and end round if a combination was chosen by the user
            if (chosenCombination != -1)
            {
                setCombination(chosenCombination);
                checkScoreCardStatus();
                if (GameHasEnded == true)
                {
                    endGame();
                }
                else
                {
                    endRound();
                }
            }

            //check if round needs to end
            if (RoundNumber < Rulebook.MAX_ROUNDS && chosenCombination == -1)
            {
                RoundIsInProgress = true;
                StartNewRound(values);
            }
            else if (RoundIsInProgress && GameHasEnded == false)
            {
                endRound();
            }
            else
            {
                StartNewTurn();
            }
        }
        public void StartNewTurn()
        {
            cup.ResetDies();
            RoundNumber = 1;
            RoundIsInProgress = true;
        }

        public void StartNewRound(bool[] values)
        {
            LockDies(values);
            cup.Shuffle();
            RoundNumber++;
        }

        //låser låste terninger op og evaluerer kombination
        private void endRound()
        {
            RoundIsInProgress = false;
        }

        private void endGame()
        {
            
        }

        private void endRound(int indexOfChosenCombination)
        {
            //Brug index til at sætte kombination
            cup.ResetDies();
            RoundIsInProgress = false;
        }

        private void setCombination(int value)
        {
            if (value <= 5)
            {
                playerOne.setScoreCardValue(value, ReturnSinglesValues(value));
            }
            else
            {
                playerOne.setScoreCardValue(value, ReturnCombinationValues(value - 6));
            }
        }

        private void checkScoreCardStatus()
        {
            //check om pladen er fuld
        }

        private void LockDies(bool[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == true)
                {
                    cup.DiceArray[i].IsLocked = true;
                }
            }
            //cup.DiceArray[value].IsLocked = true;
        }

        public int[] ReturnDiceValues()
        {
            for (int i = 0; i < diceValues.Length; i++)
            {
                diceValues[i] = cup.DiceArray[i].DiceValue;
            }
            return diceValues;
        }

        //returnerer pointantal for en given mængde møngde af terninger af samme værdi - tjek om resultater er i scorecard if so: returner istedet scorecardvalue
        public int ReturnSinglesValues(int dicevalue)
        {
            int points = Rulebook.GetSinglesValue(dicevalue, cup.GetOccurencesOfDiceValue(dicevalue));       
            return points;
        }

        public int ReturnCombinationValues(int indexvalue) //lav check for at se om værdi er i Scorecard - if so: returner scorecardværdi
        {
            int[] values = new int[]
            {
                Rulebook.GetOnePairValue(diceValues),
                Rulebook.GetTwoPairValue(diceValues),
                Rulebook.GetThreeOfAKindValue(diceValues),
                Rulebook.GetFourOfAKindValue(diceValues),
                Rulebook.GetFullHouseValue(diceValues),
                Rulebook.GetSmallStraightValue(diceValues),
                Rulebook.GetLargeStraightValue(diceValues),
                Rulebook.GetChanceVValue(diceValues),
                Rulebook.GetYatzeeValue(diceValues)
            };
            return values[indexvalue];
        }

        #endregion

    }
}
