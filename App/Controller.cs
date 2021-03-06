using System;

namespace App
{
    public class Controller
    {
        View view;
        GuessModel model;

        public Controller(View v, GuessModel m)
        {
           this.view = v;
           this.model = m;
        } 

        public virtual bool ActionController()
        {
            view.ShowMenu();

            View.StartMenuAction input = view.AskForAction();

            switch (input)
            {
                case View.StartMenuAction.StartNew:
                    view.ShowStartGuessingMessage();
                    PlayGame();
                    return true;
                case View.StartMenuAction.Exit:
                    return false;
                default:
                    return false;
            }
        }

        public virtual bool PlayGame()
        {
            do {
                int guess = view.GetGuessedNumber();
                model.GuessNumber(guess);
                view.ShowGameOutcome(model.GetActual(), guess, model.SetRemainingGuesses());
            } 
            while(!model.HasWon() && model.GetRemainingGuesses() != 0);
            return false;
        }
    }
}