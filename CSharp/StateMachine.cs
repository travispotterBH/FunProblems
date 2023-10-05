   /*
    Part 1
Implement a state machine called HarzardStateMachine with the following requirements.
1) The state machine has 40 levels (states).
2) The state machine has 4 level bands, a band is a logical group of possible states.
a. Levels 0-9: LowRisk
b. Levels 10-19: MediumRisk
c. Levels 20-29: HighRisk
d. Levels 30-39: VeryHighRisk
3) The state machine transitions have the following rules.
a. Once it is in the band VeryHighRisk (level 30-39), you can only go back to level 0.
b. Once you are in the band HighRisk (level 20-29), you can only go up to any level in band 4.
c. If it is at level 28, you can only go to level 29, and only then can it go back to any level in band Low.
d. Otherwise, it can go up or down to another state.
Part 2
Write a transition method for the HarzardStateMachine.
1) Takes the desired level as an input.
2) Transitions the state machine.
3) Outputs:
a. The previous level transitioned from.
b. The previous level band.
c. The current level.
d. The current level band.
*/
using System;
using StateMachine;

class Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            TestTransition();
            TestInvalidLevel();
            TestGetCurrentState();
        }

        static void TestTransition()
        {
            HazardStateMachine stateMachine = new HazardStateMachine(0);
            StateResult result; 

            result = stateMachine.Transition(10);
            AssertEquals(result.CurrentLevel, 10, "TestTransition - Transition to level 10 from level 0.");

            result = stateMachine.Transition(28);
            AssertEquals(result.CurrentLevel, 28, "TestTransition - Transition to level 28 from level 10.");

            result = stateMachine.Transition(30);
            AssertEquals(result.CurrentLevel, 28, "TestTransition - No transition to level 30 to from level 28");

            result = stateMachine.Transition(29);
            AssertEquals(result.CurrentLevel, 29, "TestTransition - Transition to level 29 from level 28");

            result = stateMachine.Transition(5);
            AssertEquals(result.CurrentLevel, 5, "TestTransition - Transition from level 29 to level 5");

            result = stateMachine.Transition(31);
            AssertEquals(result.CurrentLevel, 31, "TestTransition - Transition to level 31");

            result = stateMachine.Transition(0);
            AssertEquals(result.CurrentLevel, 0, "TestTransition - Transition from level 31 to level 0");
        }

        static void TestInvalidLevel()
        {
            try
            {
                HazardStateMachine stateMachine = new HazardStateMachine(40);
                Console.WriteLine("TestInvalidLevel - Failed: Exception should have been thrown");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("TestInvalidLevel - Passed");
            }
        }

        static void TestGetCurrentState()
        {
            HazardStateMachine stateMachine = new HazardStateMachine(10);
            StateResult currentState = stateMachine.GetCurrentState();
            AssertEquals(currentState.CurrentLevel, 10, "TestGetCurrentState - Initial level");

            stateMachine.Transition(20);
            currentState = stateMachine.GetCurrentState();
            AssertEquals(currentState.CurrentLevel, 20, "TestGetCurrentState - After transition to level 20");
        }

        static void AssertEquals(int actual, int expected, string testName)
        {
            if (actual == expected)
            {
                Console.WriteLine($"{testName} - Passed");
            }
            else
            {
                Console.WriteLine($"{testName} - Failed: Expected {expected} but got {actual}");
            }
        }
    }
}

namespace StateMachine
{
    public class HazardStateMachine
    {
        private int _currentLevel = 0;

        public HazardStateMachine(int initialLevel)
        {
            if (!IsValidLevel(initialLevel)){
                throw new ArgumentOutOfRangeException(nameof(initialLevel), "Level must be between 0 - 39.");
            }

            _currentLevel = initialLevel;
        }

        public StateResult Transition(int targetLevel)
        {
            if (!IsValidLevel(targetLevel)){
                throw new ArgumentOutOfRangeException(nameof(targetLevel), "Level must be between 0 - 39.");
            }

            int previousLevel = _currentLevel;
            
            if (IsTransitionValid(targetLevel))
            {
                _currentLevel = targetLevel;
            }

            return GetCurrentState(previousLevel);
        }

        public StateResult GetCurrentState(int previousLevel = -1)
        {
            return new StateResult(previousLevel == -1 ? _currentLevel : previousLevel, _currentLevel);
        }

        private bool IsTransitionValid(int targetLevel)
        {
            Risk currentBand = StateResult.GetLevelBand(_currentLevel);
            Risk targetBand = StateResult.GetLevelBand(targetLevel);
            
            switch (currentBand)
            {
                case Risk.VeryHighRisk:
                    return targetLevel == 0;
                case Risk.HighRisk when _currentLevel == 28:
                    return targetLevel == 29;
                case Risk.HighRisk when _currentLevel == 29:
                    return targetBand == Risk.LowRisk;
                case Risk.HighRisk:
                    return targetBand == Risk.VeryHighRisk;
                default:
                    return true;
            }
        }

        private bool IsValidLevel(int level) {
            return level >= 0 && level <= 39;
        }
    }

    public class StateResult
    {
        public int PreviousLevel { get; }
        public Risk PreviousLevelBand 
        {
            get 
            {
                return  GetLevelBand(PreviousLevel);
            }
        }
        public int CurrentLevel { get; }
        public Risk CurrentLevelBand
        {
            get
            {
                return GetLevelBand(CurrentLevel);
            }
        } 

        public StateResult(int previousLevel, int currentLevel)
        {
            PreviousLevel = previousLevel;
            CurrentLevel = currentLevel;
        }

        public static Risk GetLevelBand(int level)
        {
            return level switch
            {
                < 10 => Risk.LowRisk,
                < 20 => Risk.MediumRisk,
                < 30 => Risk.HighRisk,
                _ => Risk.VeryHighRisk
            };
        } 
    }

    public enum Risk
    {
        LowRisk,
        MediumRisk,
        HighRisk,
        VeryHighRisk
    }
}

/*

class HazardStateMachine
inputs: desired levle
        - int with early fail boundary check
        - switch statement  for level bands

outputs:
    - class StateResult


    methods
        constructor -- initial level
            checks on the vlaid level


        transition method 
        

        query to ge the current state
*/

